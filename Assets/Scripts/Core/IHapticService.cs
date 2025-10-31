namespace KoksalBaba.Core
{
    /// <summary>
    /// Interface for haptic feedback (iOS Taptic Engine).
    /// </summary>
    public interface IHapticService
    {
        /// <summary>
        /// Initialize the haptic service.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Trigger light haptic feedback (tap, pickup).
        /// </summary>
        void TriggerLight();

        /// <summary>
        /// Trigger medium haptic feedback (rage dash).
        /// </summary>
        void TriggerMedium();

        /// <summary>
        /// Trigger error haptic feedback (collision death).
        /// </summary>
        void TriggerError();

        /// <summary>
        /// Enable or disable haptics.
        /// </summary>
        void SetEnabled(bool enabled);
    }
}
