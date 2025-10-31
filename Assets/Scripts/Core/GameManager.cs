using UnityEngine;
using System;
using System.Collections.Generic;

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
            // TODO: Register services with ServiceLocator
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
                    // TODO: Load MainMenu scene additively
                    break;
                case GameState.Playing:
                    // TODO: Load Game scene, start spawner
                    break;
                case GameState.Paused:
                    // TODO: Set Time.timeScale = 0, show pause overlay
                    break;
                case GameState.GameOver:
                    // TODO: Stop spawner, show interstitial ad (if eligible)
                    break;
                case GameState.Results:
                    // TODO: Display results screen with final score
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
            // TODO: Log AppClose analytics event with session duration
        }
    }
}
