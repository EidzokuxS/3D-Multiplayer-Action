using UnityEngine;
using UnityEngine.UI;

namespace Arcade3D
{
    public class UIPlayerNameInput : MonoBehaviour
    {
        #region Properties

        [SerializeField] private InputField _nicknameInputField;
        [SerializeField] private Button _continueButton;
        public static string DisplayName { get; private set; }

        #endregion

        private const string PlayerPrefsNameKey = "PlayerName";

        #region Unity Events
        private void Start()
        {
            SetUpInputField();
        }

        #endregion

        #region Public API
        public void SetPlayerName(string name) => _continueButton.interactable = !string.IsNullOrEmpty(name);

        public void SavePlayerName()
        {
            DisplayName = _nicknameInputField.text;
            PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
        }

        #endregion

        #region Private API

        private void SetUpInputField()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
                return;

            string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
            _nicknameInputField.text = defaultName;
            SetPlayerName(defaultName);
        }

        #endregion
    }

}
