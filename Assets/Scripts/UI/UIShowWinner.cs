using UnityEngine;
using UnityEngine.UI;

namespace Arcade3D
{
    [RequireComponent(typeof(Text))]
    public class UIShowWinner : MonoBehaviour
    {
        private Text _winnerText;

        #region Unity Events

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            HandleQuittingGame();
        }

        #endregion

        #region Private API
        private void Hide() => gameObject.SetActive(false);

        private void Show(string winner)
        {
            gameObject.SetActive(true);
            _winnerText.text = $"{winner} won!";
        }

        private void Initialize()
        {
            _winnerText = GetComponent<Text>();
            Hide();
            RoundSystem.OnRoundRestart += Hide;
            RoundSystem.OnWinnerDetermined += Show;
        }

        private void HandleQuittingGame()
        {
            RoundSystem.OnRoundRestart -= Hide;
            RoundSystem.OnWinnerDetermined -= Show;
        }

        #endregion
    }
}

