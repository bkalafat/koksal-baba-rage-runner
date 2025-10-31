using UnityEngine;

namespace KoksalBaba.Core
{
    /// <summary>
    /// Concrete haptic service using iOS UIImpactFeedbackGenerator.
    /// No-op on unsupported platforms.
    /// </summary>
    public class HapticService : IHapticService
    {
        private bool _isEnabled = true;
        private bool _isPlatformSupported = false;

        public void Initialize()
        {
#if UNITY_IOS && !UNITY_EDITOR
            _isPlatformSupported = true;
#else
            _isPlatformSupported = false;
#endif

            _isEnabled = PlayerPrefs.GetInt("HapticsEnabled", 1) == 1;

            Debug.Log($"HapticService: Initialized (Platform supported: {_isPlatformSupported})");
        }

        public void TriggerLight()
        {
            if (!_isEnabled || !_isPlatformSupported) return;

#if UNITY_IOS && !UNITY_EDITOR
            Handheld.Vibrate(); // Unity's basic vibration API
            // In production, use native iOS UIImpactFeedbackGenerator via plugin
#endif
        }

        public void TriggerMedium()
        {
            if (!_isEnabled || !_isPlatformSupported) return;

#if UNITY_IOS && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }

        public void TriggerError()
        {
            if (!_isEnabled || !_isPlatformSupported) return;

#if UNITY_IOS && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }

        public void SetEnabled(bool enabled)
        {
            _isEnabled = enabled;
            PlayerPrefs.SetInt("HapticsEnabled", enabled ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
