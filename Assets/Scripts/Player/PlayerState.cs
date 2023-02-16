using Mirror;
using System.Collections;
using UnityEngine;

namespace Arcade3D
{
    public class PlayerState : NetworkBehaviour
    {
        #region Properties

        [SerializeField] private PlayerData _playerData;
        [SerializeField] private LayerMask _ground;

        private readonly float _heightScale = 0.5f;
        private readonly float _adjustRayLength = 0.2f;

        #endregion

        #region States       

        [SyncVar(hook = nameof(HandleColorChange))]
        private bool _immune;

        public bool Dashing { get; private set; }
        public bool Grounded { get; private set; }
        public bool Immune => _immune;
        public float DashCooldownTimer { get; private set; }

        #endregion

        #region Unity Events
        private void Update()
        {
            CheckGround();
            SetDashCooldownTimer();
        }

        #endregion

        #region Public API
        public void ChangeDashState() => Dashing = !Dashing;
        public IEnumerator SetImmutable()
        {
            _immune = true;
            yield return new WaitForSeconds(_playerData._immuneTimer);
            _immune = false;
        }

        #endregion

        #region Private API
        private void CheckGround()
        {
            Grounded = Physics.Raycast(transform.position, Vector3.down, _playerData._playerHeight * _heightScale + _adjustRayLength, _ground);
        }
        private void HandleColorChange(bool old, bool newL)
        {
            ChangePlayerColor();
        }
        private void ChangePlayerColor()
        {
            if (Immune)
                GetComponentInChildren<Renderer>().material.color = Color.red;
            if (!Immune)
                GetComponentInChildren<Renderer>().material.color = Color.white;
        }

        private void SetDashCooldownTimer()
        {
            if (DashCooldownTimer > 0)
                DashCooldownTimer -= Time.deltaTime;

            if (Dashing && DashCooldownTimer <= 0)
                DashCooldownTimer = _playerData._dashCooldown;
        }

        #endregion
    }
}
