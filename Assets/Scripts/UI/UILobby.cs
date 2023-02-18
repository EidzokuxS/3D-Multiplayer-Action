using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Arcade3D
{
    public class UILobby : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Text[] _playerNameText;
        [SerializeField] private Text[] _playerReadyText;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _readyButton;

        public Text[] PlayerNameText => _playerNameText;

        #endregion

        #region Public API
        public void SetConnectedPlayerName(int textIndex, string name) => _playerNameText[textIndex].text = name;

        public void SetPlayerReadiness(int netId) => _playerReadyText[netId - 1].text = string.Empty;

        public void SetReadyButtonOnConnect(UnityAction action) => _readyButton.onClick.AddListener(action);

        public void ResetReadyButtonOnDisconnect() => _readyButton.onClick.RemoveAllListeners();

        public void SetStartButtonActive(bool status) => _startGameButton.gameObject.SetActive(status);

        public void SetPlayerReadiness(int netId, bool isReady)
        {
            _playerReadyText[netId - 1].text = isReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }

        #endregion
    }
}
