using System;
using System.Collections.Generic;
using UnityEngine;
using KoksalBaba.Services;

namespace KoksalBaba.Core
{
    /// <summary>
    /// Stub implementations for monetization and analytics services.
    /// Replace with real implementations when integrating Unity Gaming Services.
    /// </summary>

    public class StubAnalyticsService : IAnalyticsService
    {
        public bool IsInitialized => true;

        public void Initialize()
        {
            Debug.Log("StubAnalyticsService: Initialized (no-op)");
        }

        public void LogEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            Debug.Log($"StubAnalyticsService: Event '{eventName}' logged (no-op)");
        }

        public void SetUserProperty(string propertyName, string value)
        {
            Debug.Log($"StubAnalyticsService: User property '{propertyName}' set to '{value}' (no-op)");
        }
    }

    public class StubAdService : IAdService
    {
        public bool IsInitialized => true;

        public void Initialize()
        {
            Debug.Log("StubAdService: Initialized (no-op)");
        }

        public void ShowInterstitial(Action onClosed = null, Action onFailed = null)
        {
            Debug.Log("StubAdService: ShowInterstitial called (no-op)");
            onClosed?.Invoke();
        }

        public void ShowRewarded(Action onRewardEarned = null, Action onClosed = null, Action onFailed = null)
        {
            Debug.Log("StubAdService: ShowRewarded called (no-op)");
            onFailed?.Invoke();
        }

        public bool IsRewardedLoaded()
        {
            return false;
        }
    }

    public class StubIAPService : IIAPService
    {
        public bool IsInitialized => true;

        public void Initialize()
        {
            Debug.Log("StubIAPService: Initialized (no-op)");
        }

        public void BuyProduct(string productId, Action<bool> onComplete)
        {
            Debug.Log($"StubIAPService: BuyProduct '{productId}' called (no-op)");
            onComplete?.Invoke(false);
        }

        public void RestorePurchases(Action<bool> onComplete)
        {
            Debug.Log("StubIAPService: RestorePurchases called (no-op)");
            onComplete?.Invoke(false);
        }

        public bool IsProductOwned(string productId)
        {
            return false;
        }
    }
}
