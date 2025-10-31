using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KoksalBaba.UI
{
    /// <summary>
    /// Settings overlay controller.
    /// </summary>
    public class SettingsController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private Toggle hapticsToggle;
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private Toggle analyticsConsentToggle;
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private TextMeshProUGUI restartRequiredText;

        private void Start()
        {
            soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
            hapticsToggle.onValueChanged.AddListener(OnHapticsToggleChanged);
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            analyticsConsentToggle.onValueChanged.AddListener(OnAnalyticsConsentChanged);
            closeButton.onClick.AddListener(OnCloseClicked);

            LoadSettings();
            settingsPanel.SetActive(false);
            restartRequiredText.gameObject.SetActive(false);
        }

        private void LoadSettings()
        {
            soundToggle.isOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
            hapticsToggle.isOn = PlayerPrefs.GetInt("HapticsEnabled", 1) == 1;
            analyticsConsentToggle.isOn = PlayerPrefs.GetInt("AnalyticsConsent", 1) == 1;

            // Language: 0 = en-US, 1 = tr-TR
            string currentLang = PlayerPrefs.GetString("Language", "en-US");
            languageDropdown.value = currentLang == "tr-TR" ? 1 : 0;
        }

        public void Show()
        {
            settingsPanel.SetActive(true);
        }

        public void Hide()
        {
            settingsPanel.SetActive(false);
        }

        private void OnSoundToggleChanged(bool enabled)
        {
            PlayerPrefs.SetInt("SoundEnabled", enabled ? 1 : 0);
            PlayerPrefs.Save();
            
            // Update AudioService
            var audioService = Core.ServiceLocator.Instance.Get<Core.IAudioService>();
            audioService?.SetMuted(!enabled);
        }

        private void OnHapticsToggleChanged(bool enabled)
        {
            PlayerPrefs.SetInt("HapticsEnabled", enabled ? 1 : 0);
            PlayerPrefs.Save();

            // Update HapticService
            var hapticService = Core.ServiceLocator.Instance.Get<Core.IHapticService>();
            hapticService?.SetEnabled(enabled);
        }

        private void OnLanguageChanged(int index)
        {
            string newLang = index == 1 ? "tr-TR" : "en-US";
            PlayerPrefs.SetString("Language", newLang);
            PlayerPrefs.Save();

            // Show restart required message
            restartRequiredText.gameObject.SetActive(true);
        }

        private void OnAnalyticsConsentChanged(bool enabled)
        {
            PlayerPrefs.SetInt("AnalyticsConsent", enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void OnCloseClicked()
        {
            Hide();
        }
    }
}
