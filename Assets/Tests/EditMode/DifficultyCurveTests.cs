using NUnit.Framework;
using UnityEngine;
using KoksalBaba.Scripts.Data;

namespace KoksalBaba.Tests.EditMode
{
    /// <summary>
    /// Edit Mode tests for DifficultyCurve validation.
    /// </summary>
    public class DifficultyCurveTests
    {
        [Test]
        public void DifficultyCurveSpawnPeriodDecreasesMonotonically()
        {
            // Arrange
            var curve = ScriptableObject.CreateInstance<DifficultyCurve>();
            
            // Configure curve to decrease spawn period over time
            Keyframe[] keys = new Keyframe[]
            {
                new Keyframe(0f, 1.25f),
                new Keyframe(60f, 0.95f),
                new Keyframe(120f, 0.75f)
            };
            curve.spawnPeriodCurve = new AnimationCurve(keys);

            // Act
            float t0 = curve.GetSpawnPeriod(0f);
            float t30 = curve.GetSpawnPeriod(30f);
            float t60 = curve.GetSpawnPeriod(60f);
            float t120 = curve.GetSpawnPeriod(120f);

            // Assert
            Assert.Greater(t0, t30, "Spawn period should decrease from 0s to 30s");
            Assert.Greater(t30, t60, "Spawn period should decrease from 30s to 60s");
            Assert.Greater(t60, t120, "Spawn period should decrease from 60s to 120s");

            // Cleanup
            Object.DestroyImmediate(curve);
        }

        [Test]
        public void DifficultyCurveObstacleSpeedIncreasesMonotonically()
        {
            // Arrange
            var curve = ScriptableObject.CreateInstance<DifficultyCurve>();
            
            // Configure curve to increase obstacle speed over time
            Keyframe[] keys = new Keyframe[]
            {
                new Keyframe(0f, 6.0f),
                new Keyframe(60f, 7.2f),
                new Keyframe(120f, 8.4f)
            };
            curve.obstacleSpeedCurve = new AnimationCurve(keys);

            // Act
            float t0 = curve.GetObstacleSpeed(0f);
            float t60 = curve.GetObstacleSpeed(60f);
            float t120 = curve.GetObstacleSpeed(120f);

            // Assert
            Assert.Less(t0, t60, "Obstacle speed should increase from 0s to 60s");
            Assert.Less(t60, t120, "Obstacle speed should increase from 60s to 120s");

            // Cleanup
            Object.DestroyImmediate(curve);
        }
    }
}
