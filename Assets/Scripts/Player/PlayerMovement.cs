using UnityEngine;

namespace Arcade3D
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Properties
        [SerializeField] private PlayerData _playerData;

        private PlayerState _playerState;
        private PlayerInputHandler _inputHandler;
        private Rigidbody _rigidbody;
        private ThirdPersonCamera _playerCamera;

        private Vector2 _moveInput;
        private Vector3 _dashStartPosition;
        private Vector3 _moveDirection;

        #endregion

        #region Unity Events
        private void Awake()
        {
            InitializeComponents();
        }

        private void OnCollisionEnter(Collision collision)
        {
            CheckDashInterruption(collision);
        }

        #endregion

        #region Public API
        public void Move(Vector3 forward, Vector3 right)
        {
            _moveInput = _inputHandler.MovementInput;
            _moveDirection = forward * _moveInput.y + right * _moveInput.x;
            _rigidbody.MovePosition(transform.position + _playerData._movementVelocity * Time.fixedDeltaTime * _moveDirection);
        }

        public void Jump()
        {
            if (_inputHandler.JumpInput && _playerState.Grounded)
            {
                _rigidbody.AddForce(Vector3.up * _playerData._jumpForce, ForceMode.Impulse);
            }
        }

        public void Dash(Vector3 forward)
        {
            if (_inputHandler.DashInput && _playerState.DashCooldownTimer <= 0)
            {
                PerformDash(forward);
            }

            if (_playerState.Dashing)
            {
                LimitDash();
            }

            if (_playerState.Dashing && _rigidbody.velocity.magnitude < 0.1f)
            {
                _playerState.ChangeDashState();
                _rigidbody.velocity = Vector3.zero;
            }
        }

        public void GetCameraDirection(out Vector3 forward, out Vector3 right)
        {
            forward = _playerCamera.transform.forward;
            forward.y = 0.0f;
            forward.Normalize();
            right = _playerCamera.transform.right;
            right.y = 0.0f;
            right.Normalize();
        }

        #endregion

        #region Private API
        private bool PerformDash(Vector3 forward)
        {
            if (_playerState.Dashing || _playerState.DashCooldownTimer > 0)
                return false;

            _playerState.ChangeDashState();
            _dashStartPosition = transform.position;

            if (_moveDirection == Vector3.zero)
                _rigidbody.AddForce(forward * _playerData._dashForce, ForceMode.Impulse);
            else
                _rigidbody.AddForce(_moveDirection * _playerData._dashForce, ForceMode.Impulse);

            return true;
        }

        private void LimitDash()
        {
            float distance = Vector3.Distance(_dashStartPosition, transform.position);
            if (distance >= _playerData._dashRange)
            {
                _playerState.ChangeDashState();
                _rigidbody.velocity = Vector3.zero;
                return;
            }
        }

        private void CheckDashInterruption(Collision collision)
        {
            if (_playerState.Dashing)
            {
                float dot = Vector3.Dot(_rigidbody.velocity.normalized, collision.contacts[0].normal);
                if (dot < -0.5f)
                {
                    _rigidbody.velocity = Vector3.zero;
                }
            }
        }

        private void InitializeComponents()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerState = GetComponent<PlayerState>();
            _playerCamera = GetComponent<ThirdPersonCamera>();
            _inputHandler = GetComponent<PlayerInputHandler>();
        }

        #endregion
    }
}