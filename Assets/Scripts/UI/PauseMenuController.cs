using UnityEngine;
using UnityEngine.UI;

namespace KoksalBaba.UI
{
    /// <summary>
    /// Pause menu overlay controller.
    /// </summary>
    public class PauseMenuController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private GameObject pausePanel;

        private void Start()
        {
            resumeButton.onClick.AddListener(OnResumeClicked);
            restartButton.onClick.AddListener(OnRestartClicked);
            homeButton.onClick.AddListener(OnHomeClicked);

            pausePanel.SetActive(false);
        }

        public void Show()
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Hide()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }

        private void OnResumeClicked()
        {
            // TODO: Play button tap SFX
            Hide();
            Core.GameManager.Instance.TransitionTo(Core.GameManager.GameState.Playing);
        }

        private void OnRestartClicked()
        {
            // TODO: Play button tap SFX
            Hide();
            Core.GameManager.Instance.TransitionTo(Core.GameManager.GameState.Playing);
        }

        private void OnHomeClicked()
        {
            // TODO: Play button tap SFX
            Hide();
            Core.GameManager.Instance.TransitionTo(Core.GameManager.GameState.MainMenu);
        }
    }
}
