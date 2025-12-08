using System;
using Microsoft.Xna.Framework;

namespace MonogameTest;

// Quits the game
public class QuitCommand : ICommand
{
    private Game Game;
    private QuitCommand() { }
    public QuitCommand(Game game)
    {
        Game = game;
    }
    public void Execute()
    {
        Game.Exit();
    }
}