namespace KoksalBaba.Core
{
    /// <summary>
    /// Interface for time-related queries.
    /// Abstracts Unity Time for testing and time manipulation.
    /// </summary>
    public interface ITimeService
    {
        /// <summary>
        /// Time elapsed since the start of the game (Time.time).
        /// </summary>
        float Time { get; }

        /// <summary>
        /// Time elapsed since the last frame (Time.deltaTime).
        /// </summary>
        float DeltaTime { get; }

        /// <summary>
        /// Unscaled time elapsed since the last frame (Time.unscaledDeltaTime).
        /// </summary>
        float UnscaledDeltaTime { get; }

        /// <summary>
        /// Current time scale (Time.timeScale).
        /// </summary>
        float TimeScale { get; set; }
    }
}
