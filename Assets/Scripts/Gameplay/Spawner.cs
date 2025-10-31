using UnityEngine;
using KoksalBaba.Data;

namespace KoksalBaba.Gameplay
{
    /// <summary>
    /// Spawns obstacles and pickups with weighted random selection and difficulty curve.
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        [Header("Spawn Configuration")]
        [SerializeField] private SpawnConfig spawnConfig;
        [SerializeField] private DifficultyCurve difficultyCurve;

        [Header("Spawn Parameters")]
        [SerializeField] private float spawnXPosition = 15.0f;
        [SerializeField] private Vector2 spawnYRange = new Vector2(-1.6f, 1.6f);

        [Header("Pickup Spawn Rates")]
        [SerializeField] private float tauntSpawnPeriod = 4.0f;
        [SerializeField] private float coinSpawnPeriod = 8.0f;

        private ObjectPool<Transform> _obstaclePool;
        private ObjectPool<Transform> _tauntPool;
        private ObjectPool<Transform> _coinPool;

        private float _obstacleSpawnTimer = 0f;
        private float _tauntSpawnTimer = 0f;
        private float _coinSpawnTimer = 0f;
        private float _runStartTime = 0f;

        private bool _isSpawning = false;

        private void Start()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            // TODO: Create object pools for each obstacle type and pickup type
            Debug.Log("ObjectPools initialized");
        }

        private void Update()
        {
            if (!_isSpawning || Core.GameManager.Instance.CurrentState != Core.GameManager.GameState.Playing)
                return;

            float elapsedTime = Time.time - _runStartTime;

            // Spawn obstacles
            _obstacleSpawnTimer -= Time.deltaTime;
            if (_obstacleSpawnTimer <= 0f)
            {
                SpawnObstacle(elapsedTime);
                _obstacleSpawnTimer = GetSpawnPeriod(elapsedTime);
            }

            // Spawn taunts
            _tauntSpawnTimer -= Time.deltaTime;
            if (_tauntSpawnTimer <= 0f)
            {
                SpawnPickup("Taunt");
                _tauntSpawnTimer = tauntSpawnPeriod;
            }

            // Spawn coins
            _coinSpawnTimer -= Time.deltaTime;
            if (_coinSpawnTimer <= 0f)
            {
                SpawnPickup("Coin");
                _coinSpawnTimer = coinSpawnPeriod;
            }
        }

        private void SpawnObstacle(float elapsedTime)
        {
            // TODO: Select obstacle type using weighted random from spawnConfig
            // TODO: Get instance from pool, set position (spawnXPosition, random Y in spawnYRange)
            // TODO: Apply current difficulty speed to obstacle
            Debug.Log($"Spawning obstacle at time {elapsedTime}");
        }

        private void SpawnPickup(string pickupType)
        {
            // TODO: Get pickup instance from pool (taunt or coin)
            // TODO: Set position (spawnXPosition, random Y in spawnYRange)
            Debug.Log($"Spawning {pickupType} pickup");
        }

        private float GetSpawnPeriod(float elapsedTime)
        {
            if (difficultyCurve != null)
            {
                return difficultyCurve.GetSpawnPeriod(elapsedTime);
            }
            return 1.25f; // Default initial spawn period
        }

        public void StartSpawning()
        {
            _isSpawning = true;
            _runStartTime = Time.time;
            _obstacleSpawnTimer = GetSpawnPeriod(0f);
            _tauntSpawnTimer = tauntSpawnPeriod;
            _coinSpawnTimer = coinSpawnPeriod;
            Debug.Log("Spawner started");
        }

        public void StopSpawning()
        {
            _isSpawning = false;
            Debug.Log("Spawner stopped");
        }

        public void ClearAll()
        {
            // TODO: Return all active obstacles and pickups to pools
            Debug.Log("All spawned objects cleared");
        }
    }
}
