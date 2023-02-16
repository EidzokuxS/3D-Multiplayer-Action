using UnityEngine;
using UnityEngine.UI;

public class UIPlayerNameInput : MonoBehaviour
{
    [SerializeField] private InputField _nicknameInputField;
    [SerializeField] private Button _continueButton;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        _nicknameInputField.text = defaultName;
        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name) { _continueButton.interactable = !string.IsNullOrEmpty(name); }

    public void SavePlayerName()
    {
        DisplayName = _nicknameInputField.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}
