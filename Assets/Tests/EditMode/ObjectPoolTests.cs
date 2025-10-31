using NUnit.Framework;
using UnityEngine;
using KoksalBaba.Gameplay;

namespace KoksalBaba.Tests.EditMode
{
    /// <summary>
    /// Edit Mode tests for ObjectPool.
    /// </summary>
    public class ObjectPoolTests
    {
        [Test]
        public void ObjectPoolPrewarmCreatesInstances()
        {
            // Arrange
            var prefab = new GameObject("TestPrefab");
            var pool = new ObjectPool<GameObject>(prefab, 5);

            // Act
            pool.Prewarm();

            // Assert
            var obj1 = pool.Get();
            var obj2 = pool.Get();
            Assert.IsNotNull(obj1, "Pool should return valid object");
            Assert.IsNotNull(obj2, "Pool should return second valid object");
            Assert.AreNotSame(obj1, obj2, "Pool should return different instances");

            // Cleanup
            Object.DestroyImmediate(prefab);
        }

        [Test]
        public void ObjectPoolReturnDeactivatesObject()
        {
            // Arrange
            var prefab = new GameObject("TestPrefab");
            var pool = new ObjectPool<GameObject>(prefab, 1);
            pool.Prewarm();

            // Act
            var obj = pool.Get();
            Assert.IsTrue(obj.activeInHierarchy, "Retrieved object should be active");
            pool.Return(obj);

            // Assert
            Assert.IsFalse(obj.activeInHierarchy, "Returned object should be deactivated");

            // Cleanup
            Object.DestroyImmediate(prefab);
        }
    }
}
