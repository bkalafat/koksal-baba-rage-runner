using UnityEngine;
using UnityEngine.UI;

namespace KoksalBaba.UI
{
    /// <summary>
    /// Manages HUD elements (score display, rage bar, pause button).
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private UnityEngine.UI.Image rageBarFill;
        [SerializeField] private GameObject rageReadyPrompt;
        [SerializeField] private UnityEngine.UI.Button pauseButton;

        private int _currentScore = 0;
        private float _currentRageFill = 0f;

        private void Start()
        {
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
            rageReadyPrompt.SetActive(false);
        }

        public void UpdateScore(int score)
        {
            _currentScore = score;
            scoreText.text = _currentScore.ToString("N0"); // Format with comma separators
        }

        public void UpdateRageBar(float fill)
        {
            _currentRageFill = Mathf.Clamp01(fill);
            rageBarFill.fillAmount = Mathf.Lerp(rageBarFill.fillAmount, _currentRageFill, Time.deltaTime * 10f);

            // Show rage ready prompt when full
            if (_currentRageFill >= 1.0f)
            {
                rageReadyPrompt.SetActive(true);
                // TODO: Add pulse animation to rage bar and prompt
            }
            else
            {
                rageReadyPrompt.SetActive(false);
            }
        }

        private void OnPauseButtonClicked()
        {
            Core.GameManager.Instance.TransitionTo(Core.GameManager.GameState.Paused);
            // TODO: Play button tap SFX and haptic
        }
    }
}
