using System;

namespace KoksalBaba.Core
{
    /// <summary>
    /// Interface for audio playback (SFX and music).
    /// </summary>
    public interface IAudioService
    {
        /// <summary>
        /// Initialize the audio service (preload SFX).
        /// </summary>
        void Initialize();

        /// <summary>
        /// Play a sound effect by name.
        /// </summary>
        void PlaySFX(string clipName);

        /// <summary>
        /// Play background music by name (looping).
        /// </summary>
        void PlayMusic(string clipName);

        /// <summary>
        /// Stop currently playing music.
        /// </summary>
        void StopMusic();

        /// <summary>
        /// Set master volume for SFX.
        /// </summary>
        void SetSFXVolume(float volume);

        /// <summary>
        /// Set master volume for music.
        /// </summary>
        void SetMusicVolume(float volume);

        /// <summary>
        /// Enable or disable all audio.
        /// </summary>
        void SetMuted(bool muted);
    }
}
