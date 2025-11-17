using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace MonogameTest
{
    public sealed class SoundManager
    {
        
        private static readonly Lazy<SoundManager> _instance = new(() => new SoundManager());
        public static SoundManager Instance => _instance.Value;

        private readonly Dictionary<string, Song> _songs = new();
        private readonly Dictionary<string, SoundEffect> _effects = new();

        private bool _isMuted = false;
        private float _previousMediaVolume = 1f;
        private float _previousEffectVolume = 1f;



        private SoundManager() { }

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

        public void LoadEffect(Game game, string key, string path)
        {
            try
            {
                _effects[key] = game.Content.Load<SoundEffect>(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load sound effect '{path}': {ex.Message}");
            }
        }

        public void ToggleMute()
        {
            _isMuted = !_isMuted;

            if (_isMuted)
            {
                // Save volumes
                _previousMediaVolume = MediaPlayer.Volume;
                _previousEffectVolume = SoundEffect.MasterVolume;

                // Mute everything
                MediaPlayer.Volume = 0f;
                SoundEffect.MasterVolume = 0f;

                Console.WriteLine("Audio muted.");
            }
            else
            {
                // Restore previous volumes
                MediaPlayer.Volume = _previousMediaVolume;
                SoundEffect.MasterVolume = _previousEffectVolume;

                Console.WriteLine("Audio unmuted.");
            }
        }


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
                Console.WriteLine($"Song with key '{key}' not found.");
            }
        }

        public void PlayEffect(string key)
        {
            if (_effects.TryGetValue(key, out SoundEffect effect))
            {
                effect.Play();
            }
            else
            {
                Console.WriteLine($"Sound effect with key '{key}' not found.");
            }
        }

        public void StopSong()
        {
            MediaPlayer.Stop();
        }

        public void PauseSong()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
        }

        public void ResumeSong()
        {
            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
        }
    }
}
