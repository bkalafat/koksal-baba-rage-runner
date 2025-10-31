using UnityEngine;

namespace KoksalBaba.Core
{
    /// <summary>
    /// Concrete implementation of ITimeService wrapping Unity Time.
    /// </summary>
    public class TimeService : ITimeService
    {
        public float Time => UnityEngine.Time.time;
        public float DeltaTime => UnityEngine.Time.deltaTime;
        public float UnscaledDeltaTime => UnityEngine.Time.unscaledDeltaTime;
        
        public float TimeScale
        {
            get => UnityEngine.Time.timeScale;
            set => UnityEngine.Time.timeScale = value;
        }
    }
}
