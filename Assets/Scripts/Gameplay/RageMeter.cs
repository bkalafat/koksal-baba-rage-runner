using UnityEngine;
using System;

namespace KoksalBaba.Gameplay
{
    /// <summary>
    /// Manages rage meter fill, decay, and dash activation.
    /// </summary>
    public class RageMeter : MonoBehaviour
    {
        [Header("Rage Parameters")]
        [SerializeField] private float gainPerTaunt = 0.25f;
        [SerializeField] private float passiveDecayPerSecond = 0.1f;
        [SerializeField] private float dashDurationSec = 0.8f;
        [SerializeField] private float speedMultiplier = 1.3f;

        private float _currentValue = 0.0f;
        private bool _isDashActive = false;

        public float CurrentValue => _currentValue;
        public bool IsFull => _currentValue >= 1.0f;
        public bool IsDashActive => _isDashActive;

        public event Action<float> OnValueChanged;
        public event Action OnDashActivated;
        public event Action OnDashEnded;

        private void Update()
        {
            if (Core.GameManager.Instance.CurrentState != Core.GameManager.GameState.Playing)
                return;

            // Passive decay
            if (_currentValue > 0 && !_isDashActive)
            {
                _currentValue -= passiveDecayPerSecond * Time.deltaTime;
                _currentValue = Mathf.Clamp01(_currentValue);
                OnValueChanged?.Invoke(_currentValue);
            }
        }

        public void AddRage(float amount)
        {
            if (_isDashActive) return;

            _currentValue += amount;
            _currentValue = Mathf.Clamp01(_currentValue);
            OnValueChanged?.Invoke(_currentValue);

            if (IsFull)
            {
                // TODO: Display "Tap to Rage!" prompt on HUD
            }
        }

        public void ActivateDash()
        {
            if (!IsFull || _isDashActive) return;

            _currentValue = 0.0f;
            _isDashActive = true;
            OnValueChanged?.Invoke(_currentValue);
            OnDashActivated?.Invoke();

            // TODO: Log RageActivated analytics event
            // TODO: Trigger PlayerController.StartDash(dashDurationSec, speedMultiplier)

            Invoke(nameof(EndDash), dashDurationSec);
        }

        private void EndDash()
        {
            _isDashActive = false;
            OnDashEnded?.Invoke();
        }

        public void Reset()
        {
            _currentValue = 0.0f;
            _isDashActive = false;
            CancelInvoke(nameof(EndDash));
            OnValueChanged?.Invoke(_currentValue);
        }
    }
}
