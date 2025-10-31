using UnityEngine;

namespace KoksalBaba.Data
{
    /// <summary>
    /// ScriptableObject defining difficulty progression over time.
    /// </summary>
    [CreateAssetMenu(fileName = "DifficultyCurve", menuName = "Rage Runner/Difficulty Curve")]
    public class DifficultyCurve : ScriptableObject
    {
        [Header("Spawn Period Curve (seconds)")]
        [Tooltip("X-axis: elapsed time (sec), Y-axis: spawn period (sec). Should be monotonically decreasing.")]
        public AnimationCurve spawnPeriodCurve = AnimationCurve.Linear(0f, 1.25f, 120f, 0.75f);

        [Header("Obstacle Speed Curve (units/sec)")]
        [Tooltip("X-axis: elapsed time (sec), Y-axis: obstacle speed (units/sec).")]
        public AnimationCurve obstacleSpeedCurve = AnimationCurve.Linear(0f, 6.0f, 120f, 8.4f);

        [Header("Minimum Gap Curve (world units)")]
        [Tooltip("X-axis: elapsed time (sec), Y-axis: min gap between obstacles (units).")]
        public AnimationCurve minGapCurve = AnimationCurve.Linear(0f, 3.5f, 120f, 2.5f);

        public float GetSpawnPeriod(float elapsedTime)
        {
            return spawnPeriodCurve.Evaluate(elapsedTime);
        }

        public float GetObstacleSpeed(float elapsedTime)
        {
            return obstacleSpeedCurve.Evaluate(elapsedTime);
        }

        public float GetMinGap(float elapsedTime)
        {
            return minGapCurve.Evaluate(elapsedTime);
        }

        private void OnValidate()
        {
            // Validate that spawn period is monotonically decreasing
            float prevValue = float.MaxValue;
            for (int i = 0; i < spawnPeriodCurve.length; i++)
            {
                float value = spawnPeriodCurve.keys[i].value;
                if (value > prevValue)
                {
                    Debug.LogWarning($"DifficultyCurve: spawn period should be monotonically decreasing. Key {i} has value {value} > {prevValue}");
                }
                prevValue = value;
            }
        }
    }
}
