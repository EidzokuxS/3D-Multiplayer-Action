using UnityEngine;
using UnityEngine.UI;

namespace Arcade3D
{
    [RequireComponent(typeof(Text))]
    public class UIWinner : MonoBehaviour
    {
        private Text _text;
        private void Start()
        {
            _text = GetComponent<Text>();
            Hide();

            RoundSystem.OnRoundRestart += Hide;
            RoundSystem.OnWinnerDetermined += Show;
        }

        private void OnDestroy()
        {
            RoundSystem.OnRoundRestart -= Hide;
            RoundSystem.OnWinnerDetermined -= Show;
        }

        private void Show(string winner)
        {
            gameObject.SetActive(true);
            _text.text = $"{winner} won!";
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }

}

