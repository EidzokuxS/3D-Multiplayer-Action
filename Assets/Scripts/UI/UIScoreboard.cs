using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade3D
{
    public class UIScoreboard : NetworkBehaviour
    {
        [SerializeField] private GameObject _scoreboardLinePrefab;
        [SerializeField] private Transform _linesHolder;

        private Dictionary<string, GameObject> _scoreboardLines = new();

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

        public override void OnStartClient()
        {
            print("hello");
            base.OnStartClient();
            Player.OnPlayerSpawned += UpdateScoreboard;
            Player.OnScoreChanged += UpdateScoreboard;
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
        }


        [Client]
        public void UpdateScoreboard()
        {
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

        [Client]
        public void ClearScoreboard()
        {
            foreach (GameObject scoreboardLine in _scoreboardLines.Values)
            {
                Destroy(scoreboardLine);
            }
            _scoreboardLines.Clear();
        }
    }
}

