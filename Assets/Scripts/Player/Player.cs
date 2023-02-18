using Mirror;
using System;
using UnityEngine;

namespace Arcade3D
{
    public class Player : NetworkBehaviour
    {
        #region Properties

        [SyncVar]
        private string _playerName;
        [SyncVar(hook = nameof(HandleScoreChange))]
        private int _score;
        private bool _isInputAllowed = true;
        private PlayerMovement _playerMovement;

        public string PlayerName => _playerName;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
            }
        }

        #endregion

        #region Actions
        public static event Action OnScoreChanged;
        public static event Action OnPlayerSpawned;
        public static event Action OnPlayerDespawned;
        #endregion

        private NetworkManagerPvP room;
        private NetworkManagerPvP Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerPvP;
            }
        }

        #region Unity API
        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        #endregion

        #region Public API
        public void SetInputAllowance(bool state) => _isInputAllowed = state;

        public void SetPlayerName(string name) => _playerName = name;
        #endregion

        #region Private API
        private void HandleMovement()
        {
            if (!isOwned || !_isInputAllowed)
            {
                return;
            }

            _playerMovement.GetCameraDirection(out Vector3 forward, out Vector3 right);
            _playerMovement.Move(forward, right);
            _playerMovement.Jump();
            _playerMovement.Dash(forward);
        }

        #endregion

        #region Client
        public override void OnStartClient()
        {
            Room.GamePlayers.Add(this);

            OnPlayerSpawned?.Invoke();
            OnScoreChanged?.Invoke();
        }

        public override void OnStopClient()
        {
            Room.GamePlayers.Add(this);

            OnPlayerDespawned?.Invoke();
            OnScoreChanged?.Invoke();
        }

        [Command]
        public void CmdAddScore() => _score++;

        [Command]
        public void CmdSetImmune(PlayerState target) => StartCoroutine(target.SetImmune());

        private void HandleScoreChange(int oldValue, int newValue) => OnScoreChanged?.Invoke();

        #endregion
    }
}