using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace MonogameTest
{
    public class SoundManager
    {
        private readonly Dictionary<string, Song> _songs = new();
        private readonly Dictionary<string, SoundEffect> _effects = new();

        public void LoadSong(Game game, string key, string path)
        {
            try
            {
                _songs[key] = game.Content.Load<Song>(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load song {path}: {ex.Message}");
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
                Console.WriteLine($"Failed to load sound effect {path}: {ex.Message}");
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
        }

        public void PlayEffect(string key)
        {
            if (_effects.TryGetValue(key, out SoundEffect effect))
            {
                effect.Play();
            }
        }

        public void StopSong()
        {
            MediaPlayer.Stop();
        }
    }
}
