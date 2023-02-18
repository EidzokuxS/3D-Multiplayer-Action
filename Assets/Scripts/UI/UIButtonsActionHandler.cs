using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Arcade3D
{
    public enum ButtonType
    {
        HOST_GAME_LOBBY,
        JOIN_GAME_LOBBY,
        START_GAME,
        MAIN_MENU,
        EXIT
    }

    public class UIButtonsActionHandler : MonoBehaviour
    {
        #region Properties

        [SerializeField] private ButtonType _buttonType;

        private NetworkManagerPvP _networkManager;
        private UIManager _uiManager;
        private Button _menuButton;

        #endregion

        #region Unity Events
        private void Start()
        {
            InitiallizeButton();
        }

        #endregion

        #region Private API
        private void OnButtonClicked()
        {
            switch (_buttonType)
            {
                case ButtonType.HOST_GAME_LOBBY:
                    HostGameLobby();
                    break;
                case ButtonType.JOIN_GAME_LOBBY:
                    JoinGameLobby();
                    break;
                case ButtonType.START_GAME:
                    StartGame();
                    break;
                case ButtonType.MAIN_MENU:
                    OpenMainMenu();
                    break;
                case ButtonType.EXIT:
                    Exit();
                    break;
                default:
                    break;
            }
        }
        private void JoinGameLobby() => _networkManager.StartClient();

        private void HostGameLobby() => _networkManager.StartHost();

        private static void Exit() => Application.Quit();

        private void OpenMainMenu()
        {
            SceneManager.LoadScene(0);
            _uiManager.SwitchCanvas(CanvasType.MainMenu);
        }

        private void StartGame()
        {
            _uiManager.SetMousePointerState(false, CursorLockMode.Locked);
            _networkManager.ServerChangeScene(_networkManager.GameplayScene);
        }

        private void InitiallizeButton()
        {
            _menuButton = GetComponent<Button>();
            _menuButton.onClick.AddListener(OnButtonClicked);
            _uiManager = UIManager.Instance;
            _networkManager = FindAnyObjectByType<NetworkManagerPvP>();
        }

        #endregion
    }
}