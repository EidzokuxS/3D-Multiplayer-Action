using Mirror;
using UnityEngine;

namespace Arcade3D
{
    public class PlayerHitEnemy : NetworkBehaviour
    {
        #region Properties
        public Player Player { get; private set; }
        public PlayerState PlayerState { get; private set; }

        #endregion

        #region Unity Events
        private void Awake()
        {
            InitializeComponents();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.collider.TryGetComponent(out PlayerHitEnemy player))
                return;

            if (player.Equals(this))
                return;

            HitEvent(player);
        }
        #endregion

        #region Private API
        private void InitializeComponents()
        {
            Player = GetComponent<Player>();
            PlayerState = GetComponent<PlayerState>();
        }

        private void HitEvent(PlayerHitEnemy target)
        {
            if (!PlayerState.Dashing || PlayerState.Immune)
                return;

            var enemy = target;

            if (enemy.PlayerState.Immune)
                return;

            Player.CmdSetImmune(enemy.PlayerState);
            Player.CmdAddScore();
        }

        #endregion
    }
}