using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace MonogameTest
{
    public sealed class SoundManager
    {
        // Singleton instance
        private static readonly Lazy<SoundManager> _instance =
            new(() => new SoundManager());
        public static SoundManager Instance => _instance.Value;

        // Storage
        private readonly Dictionary<string, Song> _songs = new();
        private readonly Dictionary<string, SoundEffect> _effects = new();

        // Mute state
        private bool _isMuted = false;
        private float _prevMediaVolume = DefaultVolume;
        private float _prevEffectVolume = DefaultVolume;

        // Volume constants
        private const float DefaultVolume = 1f;
        private const float MutedVolume = 0f;

        private SoundManager() { }

        // Load a music track
        public void LoadSong(Game game, string key, string path)
        {
            try
            {
                _songs[key] = game.Content.Load<Song>(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load song '{path}': {ex.Message}");
            }
        }

        // Load a sound effect
        public void LoadEffect(Game game, string key, string path)
        {
            try
            {
                _effects[key] = game.Content.Load<SoundEffect>(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load effect '{path}': {ex.Message}");
            }
        }

        public bool IsSongPlaying()
        {
            return MediaPlayer.State == MediaState.Playing;
        }

        // Toggle mute on/off
        public void ToggleMute()
        {
            _isMuted = !_isMuted;

            if (_isMuted)
            {
                // Save volumes
                _prevMediaVolume = MediaPlayer.Volume;
                _prevEffectVolume = SoundEffect.MasterVolume;

                // Mute
                MediaPlayer.Volume = MutedVolume;
                SoundEffect.MasterVolume = MutedVolume;
            }
            else
            {
                // Restore volumes
                MediaPlayer.Volume = _prevMediaVolume;
                SoundEffect.MasterVolume = _prevEffectVolume;
            }
        }

        // Play a music track
        public void PlaySong(string key, bool loop = true)
        {
            if (_songs.TryGetValue(key, out Song song))
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = loop;
            }
            else
            {
                Console.WriteLine($"Song '{key}' not found.");
            }
        }

        // Play a sound effect
        public void PlayEffect(string key)
        {
            if (_effects.TryGetValue(key, out SoundEffect effect))
            {
                effect.Play();
            }
            else
            {
                Console.WriteLine($"Effect '{key}' not found.");
            }
        }

        // Stop current song
        public void StopSong()
        {
            MediaPlayer.Stop();
        }

        // Pause current song
        public void PauseSong()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
        }

        // Resume paused song
        public void ResumeSong()
        {
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
        }
    }
}
