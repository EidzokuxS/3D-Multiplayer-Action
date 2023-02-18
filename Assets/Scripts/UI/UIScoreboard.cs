using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade3D
{
    public class UIScoreboard : NetworkBehaviour
    {
        #region Properties

        [SerializeField] private GameObject _scoreboardLinePrefab;
        [SerializeField] private Transform _linesHolder;

        private readonly Dictionary<string, GameObject> _scoreboardLines = new();

        #endregion

        private NetworkManagerPvP room;
        private NetworkManagerPvP Room
        {
            get
            {
                if (room != null)
                    return room;

                return room = NetworkManager.singleton as NetworkManagerPvP;
            }
        }

        #region NetworkBehaviour overrides
        public override void OnStartClient()
        {
            Player.OnPlayerSpawned += UpdateScoreboard;
            Player.OnScoreChanged += UpdateScoreboard;
        }

        public override void OnStopClient()
        {
            Player.OnPlayerSpawned -= UpdateScoreboard;
            Player.OnScoreChanged -= UpdateScoreboard;
        }

        #endregion

        #region Public API

        [Client]
        public void UpdateScoreboard()
        {
            ClearScoreboard();

            foreach (Player player in Room.GamePlayers)
            {
                if (!_scoreboardLines.ContainsKey(player.PlayerName))
                {
                    GameObject scoreboardLine = Instantiate(_scoreboardLinePrefab, _linesHolder);
                    _scoreboardLines.Add(player.PlayerName, scoreboardLine);
                }
                _scoreboardLines[player.PlayerName].GetComponent<Text>().text = player.PlayerName + ": " + player.Score;
            }
        }

        #endregion

        #region Private API

        [Client]
        private void ClearScoreboard()
        {
            foreach (GameObject scoreboardLine in _scoreboardLines.Values)
            {
                Destroy(scoreboardLine);
            }
            _scoreboardLines.Clear();
        }

        #endregion
    }
}

