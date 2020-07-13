using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] private Text _levelText;
        [SerializeField] private Text _blockCountText;
        [SerializeField] private Text _gameOverText;
        [SerializeField] private Text _ballCountText;
        [SerializeField] private Text _levelClearText;
        [SerializeField] private Text _countdownTimerText;

        [Header("Panels")]
        [SerializeField] private Image _gameOverPanel;
        [SerializeField] private Image _levelClearPanel;
        [SerializeField] private Image _timerPanel;

        public Text LevelText => _levelText;
        public Text BlockCountText => _blockCountText;
        public Text GameOverText => _gameOverText;
        public Text BallCountText => _ballCountText;
        public Text LevelClearText => _levelClearText;
        public Text CountdownTimerText => _countdownTimerText;

        public Image GameOverPanel => _gameOverPanel;
        public Image LevelClearPanel => _levelClearPanel;
        public Image TimerPanel => _timerPanel;

        public void ShowText(Text textToShow) => textToShow.enabled = true;

        public void HideText(Text textToHide) => textToHide.enabled = false;

        public void ShowPanel(Image panelToShow) => panelToShow.enabled = true;

        public void HidePanel(Image panelToHide) => panelToHide.enabled = false;

        public void SetText(Text textToSet, string value) => textToSet.text = value;
    }
}