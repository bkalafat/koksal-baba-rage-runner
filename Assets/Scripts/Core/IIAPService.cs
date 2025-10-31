using System;

namespace KoksalBaba.Services
{
    /// <summary>
    /// Interface for in-app purchases (Remove Ads, Starter Pack).
    /// </summary>
    public interface IIAPService
    {
        bool IsInitialized { get; }
        void Initialize();
        void BuyProduct(string productId, Action<bool> onComplete);
        void RestorePurchases(Action<bool> onComplete);
        bool IsProductOwned(string productId);
    }

    /// <summary>
    /// Mock implementation for testing without real IAP.
    /// </summary>
    public class MockIAPService : IIAPService
    {
        public bool IsInitialized => true;

        public void Initialize()
        {
            UnityEngine.Debug.Log("MockIAPService initialized");
        }

        public void BuyProduct(string productId, Action<bool> onComplete)
        {
            UnityEngine.Debug.Log($"MockIAPService: Purchased {productId}");
            onComplete?.Invoke(true);
        }

        public void RestorePurchases(Action<bool> onComplete)
        {
            UnityEngine.Debug.Log("MockIAPService: Purchases restored");
            onComplete?.Invoke(true);
        }

        public bool IsProductOwned(string productId)
        {
            return false;
        }
    }
}
