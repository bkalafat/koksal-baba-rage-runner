using System.Collections.Generic;

namespace RageRunner.Services
{
    /// <summary>
    /// Interface for analytics event logging (Firebase, Unity Analytics).
    /// </summary>
    public interface IAnalyticsService
    {
        bool IsInitialized { get; }
        void Initialize();
        void LogEvent(string eventName, Dictionary<string, object> parameters = null);
        void SetUserProperty(string propertyName, string value);
    }

    /// <summary>
    /// Mock implementation for testing without real analytics backend.
    /// </summary>
    public class MockAnalyticsService : IAnalyticsService
    {
        public bool IsInitialized => true;
        private readonly List<string> _loggedEvents = new List<string>();

        public List<string> LoggedEvents => _loggedEvents;

        public void Initialize()
        {
            UnityEngine.Debug.Log("MockAnalyticsService initialized");
        }

        public void LogEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            // Check consent
            if (UnityEngine.PlayerPrefs.GetInt("AnalyticsConsent", 1) == 0)
            {
                return; // No-op if consent disabled
            }

            _loggedEvents.Add(eventName);
            string paramStr = parameters != null ? string.Join(", ", parameters) : "none";
            UnityEngine.Debug.Log($"MockAnalyticsService: Event '{eventName}' with params: {paramStr}");
        }

        public void SetUserProperty(string propertyName, string value)
        {
            UnityEngine.Debug.Log($"MockAnalyticsService: Set property '{propertyName}' = '{value}'");
        }
    }
}
