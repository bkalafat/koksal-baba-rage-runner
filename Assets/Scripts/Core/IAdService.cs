using System;

namespace KoksalBaba.Services
{
    /// <summary>
    /// Interface for ad network operations (interstitial, rewarded video).
    /// </summary>
    public interface IAdService
    {
        bool IsInitialized { get; }
        void Initialize();
        void ShowInterstitial(Action onClosed, Action onFailed);
        void ShowRewarded(Action onRewardEarned, Action onClosed, Action onFailed);
        bool IsRewardedLoaded();
    }

    /// <summary>
    /// Mock implementation for testing without real ad networks.
    /// </summary>
    public class MockAdService : IAdService
    {
        public bool IsInitialized => true;

        public void Initialize()
        {
            UnityEngine.Debug.Log("MockAdService initialized");
        }

        public void ShowInterstitial(Action onClosed, Action onFailed)
        {
            UnityEngine.Debug.Log("MockAdService: Interstitial shown");
            onClosed?.Invoke();
        }

        public void ShowRewarded(Action onRewardEarned, Action onClosed, Action onFailed)
        {
            UnityEngine.Debug.Log("MockAdService: Rewarded video shown");
            onRewardEarned?.Invoke();
        }

        public bool IsRewardedLoaded()
        {
            return true;
        }
    }
}
