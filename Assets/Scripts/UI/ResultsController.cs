using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace KoksalBaba.UI
{
    /// <summary>
    /// Results screen UI controller.
    /// </summary>
    public class ResultsController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private TextMeshProUGUI coinsEarnedText;
        [SerializeField] private GameObject newBestBanner;
        [SerializeField] private Button replayButton;
        [SerializeField] private Button reviveButton;
        [SerializeField] private Button homeButton;

        private int _finalScore;
        private int _coinsEarned;
        private bool _isNewBest;

        private void Start()
        {
            replayButton.onClick.AddListener(OnReplayClicked);
            reviveButton.onClick.AddListener(OnReviveClicked);
            homeButton.onClick.AddListener(OnHomeClicked);
        }

        public void Show(int finalScore, int coinsEarned, bool isNewBest)
        {
            _finalScore = finalScore;
            _coinsEarned = coinsEarned;
            _isNewBest = isNewBest;

            newBestBanner.SetActive(isNewBest);

            // Animate score count-up
            StartCoroutine(CountUpScore());

            int bestScore = PlayerPrefs.GetInt("BestScore", 0);
            bestScoreText.text = $"Best: {bestScore:N0}";
            coinsEarnedText.text = $"+{coinsEarned} coins";

            // TODO: Play new best SFX and confetti if isNewBest
        }

        private IEnumerator CountUpScore()
        {
            float duration = 1.0f;
            float elapsed = 0f;
            int displayScore = 0;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                // EaseOutQuad
                t = 1 - (1 - t) * (1 - t);
                displayScore = Mathf.RoundToInt(Mathf.Lerp(0, _finalScore, t));
                finalScoreText.text = displayScore.ToString("N0");
                yield return null;
            }

            finalScoreText.text = _finalScore.ToString("N0");
        }

        private void OnReplayClicked()
        {
            // TODO: Play button tap SFX
            Core.GameManager.Instance.TransitionTo(Core.GameManager.GameState.Playing);
        }

        private void OnReviveClicked()
        {
            // TODO: Show rewarded ad, respawn on success
            Debug.Log("Revive clicked - show rewarded ad");
        }

        private void OnHomeClicked()
        {
            // TODO: Play button tap SFX
            Core.GameManager.Instance.TransitionTo(Core.GameManager.GameState.MainMenu);
        }
    }
}
