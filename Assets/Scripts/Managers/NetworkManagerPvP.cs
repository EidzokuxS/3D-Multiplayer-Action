using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Arcade3D
{
    public class NetworkManagerPvP : NetworkRoomManager
    {
        #region Fields

        [Header("UI")]
        [SerializeField] private UIManager _canvasManager;
        [SerializeField] private UILobby _lobbyUI;
        public UILobby LobbyUI => _lobbyUI;

        [Header("Game")]
        [SerializeField] private GameObject _roundSystem;

        #endregion

        #region Player lists
        public List<RoomPlayer> RoomPlayers { get; } = new List<RoomPlayer>();
        public List<Player> GamePlayers { get; } = new List<Player>();
        #endregion

        #region NetworkManager Override

        public override void OnRoomServerPlayersNotReady()
        {
            CheckReadiness();
        }

        public override void OnRoomServerPlayersReady()
        {
            CheckReadiness();
        }

        public override void OnStopServer()
        {
            RoomPlayers.Clear();
            GamePlayers.Clear();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<RoomPlayer>();
                RoomPlayers.Remove(player);
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);

            if (sceneName == GameplayScene)
            {
                GameObject roundSystemInstance = Instantiate(_roundSystem);
                NetworkServer.Spawn(roundSystemInstance);
            }
        }
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            SetupGamePlayer(conn, roomPlayer, gamePlayer);
            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        }

        public override Transform GetStartPosition()
        {
            startPositions.RemoveAll(t => t == null);

            List<Transform> activeSpots = startPositions.FindAll(x => x.gameObject.activeSelf);
            if (activeSpots.Count == 0)
                return null;

            Transform spot = activeSpots[Random.Range(0, activeSpots.Count)];
            spot.gameObject.SetActive(false);

            return spot;
        }

        #endregion

        #region Public API
        public void RespawnPlayers()
        {
            startPositions.ForEach(x => x.gameObject.SetActive(true));
            foreach (Player player in GamePlayers)
            {
                MovePlayerToSpawnPoint(player);
            }
        }
        #endregion

        #region Private API
        private void CheckReadiness()
        {
            if (RoomPlayers[0].index != 0)
                return;

            _lobbyUI.SetStartButtonActive(allPlayersReady);
        }

        private void MovePlayerToSpawnPoint(Player player)
        {
            Transform spot = GetStartPosition();
            player.SetInputAllowance(false);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.transform.position = spot.position;
            player.SetInputAllowance(true);
        }
        private void SetupGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            var player = gamePlayer.GetComponent<Player>();
            player.SetPlayerName(roomPlayer.GetComponent<RoomPlayer>().DisplayName);
            conn.identity.GetComponent<RoomPlayer>().RpcOnSceneChange(CanvasType.GameUI);
        }

        #endregion
    }
}