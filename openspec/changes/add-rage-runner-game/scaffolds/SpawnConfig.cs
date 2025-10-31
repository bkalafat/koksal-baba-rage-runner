using UnityEngine;
using System;

namespace RageRunner.Data
{
    /// <summary>
    /// ScriptableObject defining obstacle spawn weights and gap constraints.
    /// </summary>
    [CreateAssetMenu(fileName = "SpawnConfig", menuName = "Rage Runner/Spawn Config")]
    public class SpawnConfig : ScriptableObject
    {
        [Header("Obstacle Prefabs and Weights")]
        public ObstacleEntry[] obstacles;

        [Header("Gap Constraints (world units)")]
        public float minGap = 3.5f;
        public float maxGap = 6.0f;

        [Serializable]
        public class ObstacleEntry
        {
            public GameObject prefab;
            [Tooltip("Relative weight for weighted random selection (e.g., 50 = 50% chance if total weights = 100)")]
            public int weight = 1;
        }

        public GameObject SelectRandomObstacle()
        {
            int totalWeight = 0;
            foreach (var entry in obstacles)
            {
                totalWeight += entry.weight;
            }

            int randomValue = UnityEngine.Random.Range(0, totalWeight);
            int cumulativeWeight = 0;

            foreach (var entry in obstacles)
            {
                cumulativeWeight += entry.weight;
                if (randomValue < cumulativeWeight)
                {
                    return entry.prefab;
                }
            }

            return obstacles[0].prefab; // Fallback
        }

        private void OnValidate()
        {
            // Validate that total weights > 0
            int totalWeight = 0;
            foreach (var entry in obstacles)
            {
                totalWeight += entry.weight;
            }

            if (totalWeight <= 0)
            {
                Debug.LogWarning("SpawnConfig: Total obstacle weights must be > 0");
            }
        }
    }
}
