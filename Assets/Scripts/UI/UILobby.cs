using Arcade3D;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    [SerializeField] public Text[] _playerNameText;
    public Text[] PlayerNameText => _playerNameText;
    [SerializeField] private Text[] _playerReadyText;
    public Text[] PlayerReadyText => _playerReadyText;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _readyButton;

    private NetworkManagerPvP room;
    private NetworkManagerPvP Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerPvP;
        }
    }

    public void SetConnectedPlayerName(int textIndex, string name)
    {
        _playerNameText[textIndex].text = name;
    }

    public void SetPlayerReadiness(int netId)
    {
        _playerReadyText[netId - 1].text = string.Empty;
    }
    public void SetPlayerReadiness(int netId, bool isReady)
    {
        _playerReadyText[netId - 1].text = isReady ?
            "<color=green>Ready</color>" :
            "<color=red>Not Ready</color>";
    }

    public void SetPressReadyButtonEvent(UnityAction action)
    {
        _readyButton.onClick.AddListener(action);
    }
    public void ResetReadyButtonOnDisconnect()
    {
        _readyButton.onClick.RemoveAllListeners();
    }

    public void SetStartButtonActive(bool status)
    {
        _startGameButton.gameObject.SetActive(status);
    }
}
