using Microsoft.Xna.Framework;using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Animations;
using MonogameTest.Sounds;
using MonogameTest.Screens;

namespace MonogameTest
{
    public class InitalizeGame
    {
        private GraphicsDeviceManager _graphics;
        private InputController _input = new InputController();

        public IndializeGame()
        {
            _input = new InputController();
        }
    }
}