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
            var prefabObj = new GameObject("TestPrefab");
            var prefab = prefabObj.AddComponent<Transform>();
            var pool = new ObjectPool<Transform>(prefab, 5);

            // Act
            // Prewarm is called in constructor

            // Assert
            var obj1 = pool.Get();
            var obj2 = pool.Get();
            Assert.IsNotNull(obj1, "Pool should return valid object");
            Assert.IsNotNull(obj2, "Pool should return second valid object");
            Assert.AreNotSame(obj1, obj2, "Pool should return different instances");

            // Cleanup
            Object.DestroyImmediate(prefabObj);
        }

        [Test]
        public void ObjectPoolReturnDeactivatesObject()
        {
            // Arrange
            var prefabObj = new GameObject("TestPrefab");
            var prefab = prefabObj.AddComponent<Transform>();
            var pool = new ObjectPool<Transform>(prefab, 1);

            // Act
            var obj = pool.Get();
            Assert.IsTrue(obj.gameObject.activeInHierarchy, "Retrieved object should be active");
            pool.Return(obj);

            // Assert
            Assert.IsFalse(obj.gameObject.activeInHierarchy, "Returned object should be deactivated");

            // Cleanup
            Object.DestroyImmediate(prefabObj);
        }
    }
}
