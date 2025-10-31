using System;

namespace KoksalBaba.Core
{
    /// <summary>
    /// Interface for input detection.
    /// Abstracts touch/mouse input for testing and platform differences.
    /// </summary>
    public interface IInputService
    {
        /// <summary>
        /// Event fired when the player taps the screen.
        /// </summary>
        event Action OnTap;

        /// <summary>
        /// Initialize the input service.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Update input detection (call in Update loop).
        /// </summary>
        void Update();
    }
}
