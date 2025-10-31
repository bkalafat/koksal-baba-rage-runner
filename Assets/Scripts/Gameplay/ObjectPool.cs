using UnityEngine;
using System.Collections.Generic;

namespace KoksalBaba.Gameplay
{
    /// <summary>
    /// Generic object pool for reusing GameObjects (obstacles, pickups, audio sources).
    /// </summary>
    /// <typeparam name="T">Component type to pool</typeparam>
    public class ObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly int _initialSize;
        private readonly Queue<T> _available = new Queue<T>();

        public int AvailableCount => _available.Count;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _initialSize = initialSize;
            _parent = parent;

            Prewarm();
        }

        private void Prewarm()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                T instance = Object.Instantiate(_prefab, _parent);
                instance.gameObject.SetActive(false);
                _available.Enqueue(instance);
            }

            Debug.Log($"ObjectPool<{typeof(T).Name}> prewarmed with {_initialSize} instances");
        }

        public T Get()
        {
            if (_available.Count > 0)
            {
                T instance = _available.Dequeue();
                instance.gameObject.SetActive(true);
                return instance;
            }
            else
            {
                // Pool exhausted, instantiate new instance (graceful degradation)
                Debug.LogWarning($"ObjectPool<{typeof(T).Name}> exhausted, instantiating new instance");
                T instance = Object.Instantiate(_prefab, _parent);
                return instance;
            }
        }

        public void Return(T instance)
        {
            instance.gameObject.SetActive(false);
            _available.Enqueue(instance);
        }

        public void Clear()
        {
            while (_available.Count > 0)
            {
                T instance = _available.Dequeue();
                Object.Destroy(instance.gameObject);
            }
        }
    }
}
