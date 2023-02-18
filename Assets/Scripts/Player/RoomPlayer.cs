using Mirror;
using System.Collections.Generic;


namespace Arcade3D
{
    public class RoomPlayer : NetworkRoomPlayer
    {
        #region Properties

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string DisplayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsReady = false;

        private UILobby _lobbyUI;

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

        #region NetworkRoomPlayer Overrides
        public override void OnStartAuthority()
        {
            CmdSetDisplayName(UIPlayerNameInput.DisplayName);
        }
        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);
            UIManager.Instance.SwitchCanvas(CanvasType.Lobby);

            _lobbyUI = Room.LobbyUI;
            _lobbyUI.SetPlayerReadiness((int)netId, false);
            _lobbyUI.SetReadyButtonOnConnect(CmdSetReadiness);

            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            _lobbyUI.SetPlayerReadiness((int)netId);
            _lobbyUI.SetReadyButtonOnConnect(CmdSetReadiness);
            _lobbyUI.ResetReadyButtonOnDisconnect();

            Room.RoomPlayers.Remove(this);

            UpdateDisplay();
        }

        #endregion

        #region Client-Server

        [Command]
        private void CmdSetDisplayName(string displayName)
        {
            DisplayName = displayName;
        }

        [Command]
        private void CmdSetReadiness()
        {
            IsReady = !IsReady;
        }

        [ClientRpc]
        public void RpcOnSceneChange(CanvasType canvas)
        {
            UIManager.Instance.SwitchCanvas(canvas);
        }


        #endregion

        #region Private API
        private void UpdateDisplay()
        {
            List<RoomPlayer> roomPlayers = Room.RoomPlayers;

            bool foundOwner = false;
            foreach (var player in roomPlayers)
            {
                if (player.isOwned)
                {
                    foundOwner = true;
                    break;
                }
            }

            if (!foundOwner)
                return;

            for (int i = 0; i < _lobbyUI.PlayerNameText.Length; i++)
            {
                if (i < roomPlayers.Count)
                    _lobbyUI.SetConnectedPlayerName(i, roomPlayers[i].DisplayName);
                else
                    _lobbyUI.SetConnectedPlayerName(i, "Waiting For Player...");
            }
        }

        #endregion

        #region Hooks

        private void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
        private void HandleReadyStatusChanged(bool oldValue, bool newValue)
        {
            Room.RoomPlayers[(int)netId - 1].CmdChangeReadyState(newValue);
            _lobbyUI.SetPlayerReadiness((int)this.netId, newValue);
        }

        #endregion
    }

}
