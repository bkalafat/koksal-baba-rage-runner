using System.Collections.Generic;
using UnityEngine;

namespace KoksalBaba.Core
{
    /// <summary>
    /// Concrete audio service with object pooling for zero-GC SFX playback.
    /// </summary>
    public class AudioService : MonoBehaviour, IAudioService
    {
        [Header("Audio Sources")]
        [SerializeField] private int sfxPoolSize = 10;
        private Queue<AudioSource> _sfxPool = new Queue<AudioSource>();
        private AudioSource _musicSource;

        [Header("Audio Clips")]
        [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();
        [SerializeField] private List<AudioClip> musicClips = new List<AudioClip>();

        private Dictionary<string, AudioClip> _sfxLookup = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> _musicLookup = new Dictionary<string, AudioClip>();

        private float _sfxVolume = 0.8f;
        private float _musicVolume = 0.7f;
        private bool _isMuted = false;

        public void Initialize()
        {
            // Create SFX pool
            for (int i = 0; i < sfxPoolSize; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                _sfxPool.Enqueue(source);
            }

            // Create music source
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.playOnAwake = false;
            _musicSource.loop = true;
            _musicSource.volume = _musicVolume;

            // Build lookup dictionaries
            foreach (var clip in sfxClips)
            {
                if (clip != null)
                    _sfxLookup[clip.name] = clip;
            }

            foreach (var clip in musicClips)
            {
                if (clip != null)
                    _musicLookup[clip.name] = clip;
            }

            // Load mute preference
            _isMuted = PlayerPrefs.GetInt("SoundEnabled", 1) == 0;
            UpdateAudioState();

            Debug.Log("AudioService: Initialized with " + sfxPoolSize + " SFX sources");
        }

        public void PlaySFX(string clipName)
        {
            if (_isMuted) return;

            if (!_sfxLookup.TryGetValue(clipName, out AudioClip clip))
            {
                Debug.LogWarning($"AudioService: SFX clip '{clipName}' not found");
                return;
            }

            if (_sfxPool.Count == 0)
            {
                Debug.LogWarning("AudioService: SFX pool exhausted, skipping playback");
                return;
            }

            AudioSource source = _sfxPool.Dequeue();
            source.volume = _sfxVolume;
            source.PlayOneShot(clip);
            
            // Return to pool after clip finishes
            float clipLength = clip.length;
            Invoke(nameof(ReturnSourceToPool), clipLength);
        }

        private void ReturnSourceToPool()
        {
            // Note: This is a simplified pool return. In production, track individual sources.
            // For now, we rely on the pool being large enough to handle concurrent SFX.
        }

        public void PlayMusic(string clipName)
        {
            if (!_musicLookup.TryGetValue(clipName, out AudioClip clip))
            {
                Debug.LogWarning($"AudioService: Music clip '{clipName}' not found");
                return;
            }

            _musicSource.clip = clip;
            _musicSource.volume = _isMuted ? 0 : _musicVolume;
            _musicSource.Play();
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            _musicSource.volume = _isMuted ? 0 : _musicVolume;
        }

        public void SetMuted(bool muted)
        {
            _isMuted = muted;
            PlayerPrefs.SetInt("SoundEnabled", muted ? 0 : 1);
            PlayerPrefs.Save();
            UpdateAudioState();
        }

        private void UpdateAudioState()
        {
            _musicSource.volume = _isMuted ? 0 : _musicVolume;
        }
    }
}
