using UnityEngine;
using System;

namespace KoksalBaba.Gameplay
{
    /// <summary>
    /// Tracks distance-based score, pickup bonuses, and chain multipliers.
    /// </summary>
    public class ScoreService : MonoBehaviour
    {
        [Header("Score Parameters")]
        [SerializeField] private float distanceScoreRate = 2.0f; // 1 point per 0.5 units
        [SerializeField] private int tauntBonus = 5;
        [SerializeField] private int coinBonus = 10;
        [SerializeField] private int crateBonus = 10;
        [SerializeField] private int chainBonusPerPickup = 2;
        [SerializeField] private float chainWindowSeconds = 3.0f;

        private int _currentScore = 0;
        private int _sessionCoins = 0;
        private float _distanceTraveled = 0f;
        private int _chainCounter = 0;
        private float _chainTimer = 0f;

        public int CurrentScore => _currentScore;
        public int SessionCoins => _sessionCoins;
        public int ChainCounter => _chainCounter;

        public event Action<int> OnScoreChanged;
        public event Action<int> OnChainChanged;

        private void Update()
        {
            if (Core.GameManager.Instance.CurrentState != Core.GameManager.GameState.Playing)
                return;

            // Update distance score
            _distanceTraveled += Time.deltaTime * 5.0f; // Assume forward speed 5.0
            int newScore = Mathf.FloorToInt(_distanceTraveled * distanceScoreRate);
            if (newScore != _currentScore)
            {
                _currentScore = newScore;
                OnScoreChanged?.Invoke(_currentScore);
            }

            // Update chain timer
            if (_chainCounter > 0)
            {
                _chainTimer -= Time.deltaTime;
                if (_chainTimer <= 0f)
                {
                    BreakChain();
                }
            }
        }

        public void AddTauntPickup()
        {
            int bonus = tauntBonus + (_chainCounter * chainBonusPerPickup);
            _currentScore += bonus;
            OnScoreChanged?.Invoke(_currentScore);

            IncrementChain();

            // TODO: Log TauntPickup analytics event
        }

        public void AddCoinPickup(int coinAmount = 5)
        {
            _currentScore += coinBonus;
            _sessionCoins += coinAmount;
            OnScoreChanged?.Invoke(_currentScore);

            IncrementChain();
        }

        public void AddCrateBreak()
        {
            _currentScore += crateBonus;
            OnScoreChanged?.Invoke(_currentScore);
        }

        private void IncrementChain()
        {
            _chainCounter++;
            _chainTimer = chainWindowSeconds;
            OnChainChanged?.Invoke(_chainCounter);

            if (_chainCounter >= 5)
            {
                // TODO: Display "Mega Chain!" visual effect
            }
        }

        private void BreakChain()
        {
            _chainCounter = 0;
            OnChainChanged?.Invoke(_chainCounter);
            // TODO: Display "Chain Broken!" text on HUD
        }

        public void ResetSession()
        {
            _currentScore = 0;
            _sessionCoins = 0;
            _distanceTraveled = 0f;
            _chainCounter = 0;
            _chainTimer = 0f;
            OnScoreChanged?.Invoke(_currentScore);
            OnChainChanged?.Invoke(_chainCounter);
        }

        public void FinalizeRun()
        {
            // Update best score
            Core.GameManager.Instance.UpdateBestScore(_currentScore);

            // Add session coins to total
            Core.GameManager.Instance.AddCoins(_sessionCoins);

            // TODO: Log Score analytics event
            Debug.Log($"Run finalized: Score={_currentScore}, Coins={_sessionCoins}");
        }
    }
}
