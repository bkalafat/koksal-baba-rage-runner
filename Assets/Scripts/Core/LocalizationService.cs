using System.Collections.Generic;
using UnityEngine;

namespace KoksalBaba.Core
{
    /// <summary>
    /// Localization service for managing string translations.
    /// Loads from CSV file with format: Key,tr-TR,en-US
    /// </summary>
    public class LocalizationService : MonoBehaviour
    {
        private Dictionary<string, Dictionary<string, string>> _strings = new Dictionary<string, Dictionary<string, string>>();
        private string _currentLanguage = "en-US";

        public string CurrentLanguage => _currentLanguage;

        public void Initialize()
        {
            // Detect device language
            string systemLang = Application.systemLanguage == SystemLanguage.Turkish ? "tr-TR" : "en-US";
            _currentLanguage = PlayerPrefs.GetString("Language", systemLang);

            LoadStrings();
            Debug.Log($"LocalizationService: Initialized with language {_currentLanguage}");
        }

        private void LoadStrings()
        {
            // TODO: Load from Assets/Localization/Strings.csv
            // For now, hardcode essential strings
            _strings["play"] = new Dictionary<string, string>
            {
                { "en-US", "Play" },
                { "tr-TR", "Oyna" }
            };
            _strings["shop"] = new Dictionary<string, string>
            {
                { "en-US", "Shop" },
                { "tr-TR", "Mağaza" }
            };
            _strings["settings"] = new Dictionary<string, string>
            {
                { "en-US", "Settings" },
                { "tr-TR", "Ayarlar" }
            };
            _strings["best"] = new Dictionary<string, string>
            {
                { "en-US", "Best" },
                { "tr-TR", "En İyi" }
            };
            _strings["score"] = new Dictionary<string, string>
            {
                { "en-US", "Score" },
                { "tr-TR", "Puan" }
            };
            _strings["tap_to_rage"] = new Dictionary<string, string>
            {
                { "en-US", "Tap to Rage!" },
                { "tr-TR", "Öfkelenmeye Dokun!" }
            };
            _strings["game_over"] = new Dictionary<string, string>
            {
                { "en-US", "Game Over" },
                { "tr-TR", "Oyun Bitti" }
            };
            _strings["replay"] = new Dictionary<string, string>
            {
                { "en-US", "Replay" },
                { "tr-TR", "Tekrar" }
            };
            _strings["home"] = new Dictionary<string, string>
            {
                { "en-US", "Home" },
                { "tr-TR", "Ana Sayfa" }
            };
            _strings["revive"] = new Dictionary<string, string>
            {
                { "en-US", "Revive" },
                { "tr-TR", "Canlan" }
            };
            _strings["new_best"] = new Dictionary<string, string>
            {
                { "en-US", "New Best!" },
                { "tr-TR", "Yeni Rekor!" }
            };
        }

        public string GetString(string key)
        {
            if (_strings.TryGetValue(key, out var translations))
            {
                if (translations.TryGetValue(_currentLanguage, out var text))
                {
                    return text;
                }
                // Fallback to en-US
                if (translations.TryGetValue("en-US", out var fallback))
                {
                    return fallback;
                }
            }

            Debug.LogWarning($"LocalizationService: String key '{key}' not found");
            return $"[{key}]";
        }

        public void SetLanguage(string languageCode)
        {
            _currentLanguage = languageCode;
            PlayerPrefs.SetString("Language", languageCode);
            PlayerPrefs.Save();
        }
    }
}
