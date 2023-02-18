using Mirror;
using UnityEngine;

namespace Arcade3D
{
    public class ThirdPersonCamera : NetworkBehaviour
    {
        #region Properties

        [SerializeField] private GameObject _cameraHolder;
        [SerializeField] private float _cameraDistance = 10f;
        [SerializeField] private float _mouseSensitivity = 2f;

        private float _yaw;
        private float _pitch;

        #endregion

        #region Unity Events
        private void Update()
        {
            SetLookDirection();
        }

        #endregion

        #region NetworkBehaviour overrides
        public override void OnStartAuthority() => _cameraHolder.SetActive(true);

        #endregion

        #region Private API
        private void GetLookDirection()
        {
            _yaw += Input.GetAxis("Mouse X") * _mouseSensitivity;
            _pitch -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        }

        private void SetLookDirection()
        {
            GetLookDirection();

            Vector3 targetPosition = transform.position - Quaternion.Euler(_pitch, _yaw, 0f) * Vector3.forward * _cameraDistance;

            if (Physics.Raycast(transform.position, targetPosition - transform.position, out RaycastHit hit, _cameraDistance))
            {
                targetPosition = hit.point;
            }

            _cameraHolder.transform.position = targetPosition;
            _cameraHolder.transform.LookAt(transform);

            transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
        }

        #endregion
    }
}