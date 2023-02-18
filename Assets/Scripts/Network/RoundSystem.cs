using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcade3D
{
    public class RoundSystem : NetworkBehaviour
    {

        #region Properties

        [SerializeField] private Animator _animator;
        [SerializeField] private int _winScore;

        #endregion

        #region Actions
        public static event Action<string> OnWinnerDetermined;
        public static event Action OnRoundRestart;
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

        #region Server
        public override void OnStartServer() => Player.OnScoreChanged += HandleGameState;

        public override void OnStopServer() => Player.OnScoreChanged -= HandleGameState;

        [ServerCallback]
        public void StartRound() => RpcStartRound();

        public void ShowRoundWinner(string winner) => RpcShowRoundWinner(winner);

        private void HandleGameState()
        {
            IList<Player> players = Room.GamePlayers;

            if (players.Count == 0)
                return;

            Player topPlayer = players.OrderByDescending(x => x.Score).First();
            if (topPlayer.Score < _winScore)
                return;

            ShowRoundWinner(topPlayer.PlayerName);
            ResetPlayersScore(players);
            StartCountdown();
        }

        [Server]
        private static void ResetPlayersScore(IList<Player> players)
        {
            foreach (Player player in players)
            {
                player.Score = 0;
            }
        }

        [Server]
        private void StartCountdown()
        {
            _animator.enabled = true;
            RpcStartCountdown();
        }
        public void StopCooldown()
        {
            ResetPlayersScore(Room.GamePlayers);
            _animator.enabled = false;
        }

        #endregion

        #region Client

        [ClientRpc]
        private void RpcStartCountdown() => _animator.enabled = true;

        [ClientRpc]
        private void RpcShowRoundWinner(string winnerName) => OnWinnerDetermined?.Invoke(winnerName);

        [ClientRpc]
        private void RpcStartRound()
        {
            OnRoundRestart.Invoke();
            Room.RespawnPlayers();
        }

        #endregion
    }
}