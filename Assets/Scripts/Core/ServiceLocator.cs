using System;
using System.Collections.Generic;
using UnityEngine;

namespace KoksalBaba.Core
{
    /// <summary>
    /// Simple service locator for dependency injection.
    /// Allows global access to services without tight coupling.
    /// </summary>
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceLocator();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Register a service with the locator.
        /// </summary>
        public void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"ServiceLocator: Service of type {type.Name} already registered. Replacing.");
                _services[type] = service;
            }
            else
            {
                _services.Add(type, service);
                Debug.Log($"ServiceLocator: Registered service {type.Name}");
            }
        }

        /// <summary>
        /// Retrieve a service from the locator.
        /// </summary>
        public T Get<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return service as T;
            }

            Debug.LogError($"ServiceLocator: Service of type {type.Name} not found.");
            return null;
        }

        /// <summary>
        /// Check if a service is registered.
        /// </summary>
        public bool Has<T>() where T : class
        {
            return _services.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Unregister a service (useful for tests).
        /// </summary>
        public void Unregister<T>() where T : class
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
                Debug.Log($"ServiceLocator: Unregistered service {type.Name}");
            }
        }

        /// <summary>
        /// Clear all services (useful for tests).
        /// </summary>
        public void Clear()
        {
            _services.Clear();
            Debug.Log("ServiceLocator: Cleared all services");
        }

        /// <summary>
        /// Reset the singleton instance (useful for tests).
        /// </summary>
        public static void ResetInstance()
        {
            _instance = null;
        }
    }
}
