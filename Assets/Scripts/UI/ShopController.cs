using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using KoksalBaba.Services;

namespace KoksalBaba.UI
{
    /// <summary>
    /// Shop overlay controller for IAP and cosmetics.
    /// </summary>
    public class ShopController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI totalCoinsText;
        [SerializeField] private Button removeAdsButton;
        [SerializeField] private Button starterPackButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private Transform cosmeticsGrid;
        [SerializeField] private GameObject cosmeticItemPrefab;

        [Header("Cosmetics")]
        [SerializeField] private List<CosmeticItem> cosmetics = new List<CosmeticItem>();

        [System.Serializable]
        public class CosmeticItem
        {
            public string id;
            public string displayName;
            public Sprite icon;
            public int cost;
            public bool isUnlocked;
        }

        private void Start()
        {
            removeAdsButton.onClick.AddListener(OnRemoveAdsClicked);
            starterPackButton.onClick.AddListener(OnStarterPackClicked);
            closeButton.onClick.AddListener(OnCloseClicked);

            shopPanel.SetActive(false);
            LoadCosmeticsState();
        }

        public void Show()
        {
            shopPanel.SetActive(true);
            UpdateCoinsDisplay();
            PopulateCosmeticsGrid();

            // Hide Remove Ads button if already purchased
            bool removeAdsPurchased = PlayerPrefs.GetInt("RemoveAdsPurchased", 0) == 1;
            removeAdsButton.gameObject.SetActive(!removeAdsPurchased);
        }

        public void Hide()
        {
            shopPanel.SetActive(false);
        }

        private void UpdateCoinsDisplay()
        {
            int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            totalCoinsText.text = $"{totalCoins:N0} coins";
        }

        private void LoadCosmeticsState()
        {
            foreach (var cosmetic in cosmetics)
            {
                cosmetic.isUnlocked = PlayerPrefs.GetInt($"Cosmetic_{cosmetic.id}", 0) == 1;
            }
        }

        private void PopulateCosmeticsGrid()
        {
            // Clear existing items
            foreach (Transform child in cosmeticsGrid)
            {
                Destroy(child.gameObject);
            }

            // Create cosmetic item buttons
            foreach (var cosmetic in cosmetics)
            {
                // TODO: Instantiate cosmeticItemPrefab and configure button
                Debug.Log($"Cosmetic: {cosmetic.displayName}, Cost: {cosmetic.cost}, Unlocked: {cosmetic.isUnlocked}");
            }
        }

        private void OnRemoveAdsClicked()
        {
            // TODO: Trigger IAPService.BuyProduct("remove_ads")
            var iapService = Core.ServiceLocator.Instance.Get<IIAPService>();
            if (iapService != null)
            {
                Debug.Log("Purchase Remove Ads initiated");
                // iapService.BuyProduct("remove_ads", OnPurchaseComplete);
            }
        }

        private void OnStarterPackClicked()
        {
            // TODO: Trigger IAPService.BuyProduct("starter_pack")
            var iapService = Core.ServiceLocator.Instance.Get<IIAPService>();
            if (iapService != null)
            {
                Debug.Log("Purchase Starter Pack initiated");
                // iapService.BuyProduct("starter_pack", OnPurchaseComplete);
            }
        }

        private void OnPurchaseComplete(bool success, string productId)
        {
            if (success)
            {
                Debug.Log($"Purchase successful: {productId}");
                // TODO: Award items, update UI
            }
            else
            {
                Debug.Log($"Purchase failed: {productId}");
            }
        }

        private void OnCloseClicked()
        {
            Hide();
        }

        public void UnlockCosmetic(string cosmeticId, int cost)
        {
            int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            
            if (totalCoins < cost)
            {
                Debug.Log("Not enough coins!");
                // TODO: Show "Not enough coins" error with shake animation
                return;
            }

            // Deduct coins
            totalCoins -= cost;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            
            // Unlock cosmetic
            PlayerPrefs.SetInt($"Cosmetic_{cosmeticId}", 1);
            PlayerPrefs.Save();

            UpdateCoinsDisplay();
            PopulateCosmeticsGrid();
        }
    }
}
