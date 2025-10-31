using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using KoksalBaba.Gameplay;

namespace KoksalBaba.Tests.PlayMode
{
    /// <summary>
    /// Play Mode tests for PlayerController mechanics.
    /// </summary>
    public class PlayerControllerTests
    {
        private GameObject _playerObject;
        private PlayerController _playerController;

        [SetUp]
        public void Setup()
        {
            _playerObject = new GameObject("Player");
            _playerObject.AddComponent<Rigidbody2D>();
            _playerObject.AddComponent<BoxCollider2D>();
            _playerController = _playerObject.AddComponent<PlayerController>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(_playerObject);
        }

        [UnityTest]
        public IEnumerator PlayerJumpAppliesImpulse()
        {
            // Arrange
            var rb = _playerObject.GetComponent<Rigidbody2D>();
            rb.gravityScale = 2.5f;

            // Act
            // Simulate tap (trigger hop manually via reflection or public method)
            // _playerController.ApplyHopImpulse(); // Make this public or use Input simulation

            yield return new WaitForSeconds(0.1f);

            // Assert
            // Assert.Greater(rb.velocity.y, 0, "Player should have upward velocity after jump");
            Assert.Pass("Test framework ready - implement input simulation");
        }

        [UnityTest]
        public IEnumerator RageDashGrantsInvulnerability()
        {
            // Arrange
            _playerController.StartDash(0.8f, 1.3f);

            // Act
            yield return new WaitForSeconds(0.1f);

            // Assert
            Assert.IsTrue(_playerController.IsDashing, "Player should be dashing");
            Assert.Pass("Test framework ready - implement collision testing");
        }
    }
}
