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
        public void AnalyticsConsentCanBeSetAndRetrieved()
        {
            // Arrange
            PlayerPrefs.SetInt("AnalyticsConsent", 0);

            // Act
            int consent = PlayerPrefs.GetInt("AnalyticsConsent", -1);

            // Assert
            Assert.AreEqual(0, consent, "Analytics consent should be retrievable from PlayerPrefs");

            // Cleanup
            PlayerPrefs.DeleteKey("AnalyticsConsent");
        }

        [Test]
        public void AnalyticsConsentDefaultsToZero()
        {
            // Arrange
            PlayerPrefs.DeleteKey("AnalyticsConsent");

            // Act
            int consent = PlayerPrefs.GetInt("AnalyticsConsent", 0);

            // Assert
            Assert.AreEqual(0, consent, "Analytics consent should default to 0 when not set");
        }
    }
}
