using UnityEngine;
using System;
using System.Collections.Generic;
using KoksalBaba.Services;

namespace KoksalBaba.Core
{
    /// <summary>
    /// Persistent singleton managing game state and scene transitions.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState
        {
            Bootstrap,
            MainMenu,
            Playing,
            Paused,
            GameOver,
            Results
        }

        private GameState _currentState = GameState.Bootstrap;
        public GameState CurrentState => _currentState;

        public event Action<GameState, GameState> OnStateChanged;

        // Persistent session data
        private int _bestScore = 0;
        private int _totalCoins = 0;
        private bool _removeAdsPurchased = false;

        public int BestScore => _bestScore;
        public int TotalCoins => _totalCoins;
        public bool RemoveAdsPurchased => _removeAdsPurchased;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadPersistentData();
        }

        private void Start()
        {
            // Initialize services via ServiceLocator
            InitializeServices();
            TransitionTo(GameState.MainMenu);
        }

        private void InitializeServices()
        {
            // Service initialization order: Time → Input → Analytics → Ads → IAP
            Debug.Log("Initializing services...");
            // Core services
            var timeService = new TimeService();
            ServiceLocator.Instance.Register<ITimeService>(timeService);

            var inputService = gameObject.AddComponent<TouchInputService>();
            inputService.Initialize();
            ServiceLocator.Instance.Register<IInputService>(inputService);

            var audioService = gameObject.AddComponent<AudioService>();
            audioService.Initialize();
            ServiceLocator.Instance.Register<IAudioService>(audioService);

            var hapticService = new HapticService();
            hapticService.Initialize();
            ServiceLocator.Instance.Register<IHapticService>(hapticService);

            var localizationService = gameObject.AddComponent<LocalizationService>();
            localizationService.Initialize();
            ServiceLocator.Instance.Register<LocalizationService>(localizationService);

            // Stub implementations for monetization/analytics (implement later)
            ServiceLocator.Instance.Register<IAnalyticsService>(new StubAnalyticsService());
            ServiceLocator.Instance.Register<IAdService>(new StubAdService());
            ServiceLocator.Instance.Register<IIAPService>(new StubIAPService());

            Debug.Log("All services initialized and registered.");
        }

        public void TransitionTo(GameState newState)
        {
            if (_currentState == newState) return;

            GameState oldState = _currentState;
            _currentState = newState;

            OnStateChanged?.Invoke(oldState, newState);

            Debug.Log($"GameState transition: {oldState} → {newState}");

            HandleStateTransition(newState);
        }

        private void HandleStateTransition(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                    break;
                case GameState.Playing:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    // Show pause overlay (handled by PauseMenuController in scene)
                    break;
                case GameState.GameOver:
                    Time.timeScale = 1f;
                    // Show interstitial ad if eligible (handled by ad service)
                    if (!_removeAdsPurchased)
                    {
                        ServiceLocator.Instance.Get<IAdService>()?.ShowInterstitial(null, null);
                    }
                    break;
                case GameState.Results:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Results");
                    break;
            }
        }

        public void UpdateBestScore(int newScore)
        {
            if (newScore > _bestScore)
            {
                _bestScore = newScore;
                PlayerPrefs.SetInt("BestScore", _bestScore);
                PlayerPrefs.Save();
            }
        }

        public void AddCoins(int amount)
        {
            _totalCoins += amount;
            PlayerPrefs.SetInt("TotalCoins", _totalCoins);
            PlayerPrefs.Save();
        }

        public void SetRemoveAdsPurchased(bool purchased)
        {
            _removeAdsPurchased = purchased;
            PlayerPrefs.SetInt("RemoveAdsPurchased", purchased ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadPersistentData()
        {
            _bestScore = PlayerPrefs.GetInt("BestScore", 0);
            _totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            _removeAdsPurchased = PlayerPrefs.GetInt("RemoveAdsPurchased", 0) == 1;
        }

        private void OnApplicationQuit()
        {
            var analyticsService = ServiceLocator.Instance.Get<IAnalyticsService>();
            if (analyticsService != null)
            {
                var sessionData = new Dictionary<string, object>
                {
                    { "session_duration", Time.realtimeSinceStartup },
                    { "best_score", _bestScore },
                    { "total_coins", _totalCoins }
                };
                analyticsService.LogEvent("AppClose", sessionData);
            }
        }
    }
}
