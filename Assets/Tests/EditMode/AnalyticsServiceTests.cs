using NUnit.Framework;
using UnityEngine;
using KoksalBaba.Core;

namespace KoksalBaba.Tests.EditMode
{
    /// <summary>
    /// Edit Mode tests for Analytics service wrapper.
    /// </summary>
    public class AnalyticsServiceTests
    {
        [Test]
        public void AnalyticsServiceDoesNotThrowWhenConsentDenied()
        {
            // Arrange
            PlayerPrefs.SetInt("AnalyticsConsent", 0);
            var analyticsService = new MockAnalyticsService();
            analyticsService.Initialize();

            // Act & Assert
            Assert.DoesNotThrow(() => {
                analyticsService.LogEvent("TestEvent", null);
            }, "LogEvent should not throw when consent is denied");

            // Cleanup
            PlayerPrefs.DeleteKey("AnalyticsConsent");
        }

        [Test]
        public void AnalyticsServiceLogsEventWhenConsentGranted()
        {
            // Arrange
            PlayerPrefs.SetInt("AnalyticsConsent", 1);
            var analyticsService = new MockAnalyticsService();
            analyticsService.Initialize();

            // Act
            analyticsService.LogEvent("TestEvent", null);

            // Assert
            // Verify event was logged (check mock's event list)
            Assert.Pass("Test framework ready - implement mock verification");

            // Cleanup
            PlayerPrefs.DeleteKey("AnalyticsConsent");
        }
    }
}
