using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KoksalBaba.UI
{
    /// <summary>
    /// Main menu UI controller.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private TextMeshProUGUI titleText;

        private void Start()
        {
            playButton.onClick.AddListener(OnPlayClicked);
            shopButton.onClick.AddListener(OnShopClicked);
            settingsButton.onClick.AddListener(OnSettingsClicked);

            UpdateBestScore();
            
            // TODO: Localize text elements
            titleText.text = "Köksal Baba: Rage Runner";
        }

        private void UpdateBestScore()
        {
            int bestScore = PlayerPrefs.GetInt("BestScore", 0);
            bestScoreText.text = $"Best: {bestScore:N0}";
        }

        private void OnPlayClicked()
        {
            // TODO: Play button tap SFX
            Core.GameManager.Instance.TransitionTo(Core.GameManager.GameState.Playing);
        }

        private void OnShopClicked()
        {
            // TODO: Show shop overlay
            Debug.Log("Shop clicked");
        }

        private void OnSettingsClicked()
        {
            // TODO: Show settings overlay
            Debug.Log("Settings clicked");
        }
    }
}
