using Microsoft.Xna.Framework;

namespace MonogameTest.Sounds
{
    public static class SoundLoader
    {
        public static void LoadAllSounds(Game game, SoundManager soundManager)
        {
            // === Background music ===
            soundManager.LoadSong(
                game,
                "mainTheme",
                Game1.ChristmasMode ? "Sounds/TechnoXmas" : "Sounds/01-main-theme-overworld."
            );

            soundManager.LoadSong(game, "underworld", "Sounds/02-underworld");
            soundManager.LoadSong(game, "starman", "Sounds/05-starman");
            soundManager.LoadSong(game, "levelComplete", "Sounds/06-level-complete");
            soundManager.LoadSong(game, "youreDead", "Sounds/08-you-re-dead");
            soundManager.LoadSong(game, "gameOver", "Sounds/09-game-over");
            soundManager.LoadSong(game, "gameOver2", "Sounds/10-game-over-2");
            soundManager.LoadSong(game, "intoTheTunnel", "Sounds/11-into-the-tunnel");
            soundManager.LoadSong(game, "hurry", "Sounds/13-hurry");
            soundManager.LoadSong(game, "hurryUnderground", "Sounds/14-hurry-underground-");
            soundManager.LoadSong(game, "hurryStarman", "Sounds/17-hurry-starman-");
            soundManager.LoadSong(game, "hurryOverworld", "Sounds/18-hurry-overworld-");

            // === Sound effects ===
            soundManager.LoadEffect(game, "bump", "Sounds/smb_bump");
            soundManager.LoadEffect(game, "coin", "Sounds/smb_coin");
            soundManager.LoadEffect(game, "breakBlock", "Sounds/smb_breakblock");
            soundManager.LoadEffect(game, "flagpole", "Sounds/smb_flagpole");
            soundManager.LoadEffect(game, "jumpSmall", "Sounds/smb_jumpsmall");
            soundManager.LoadEffect(game, "jumpSuper", "Sounds/smb_jump-super");
            soundManager.LoadEffect(game, "kick", "Sounds/smb_kick");
            soundManager.LoadEffect(game, "stomp", "Sounds/smb_stomp");
            soundManager.LoadEffect(game, "powerUp", "Sounds/smb_powerup");
            soundManager.LoadEffect(game, "powerUpAppears", "Sounds/smb_powerup_appears");
            soundManager.LoadEffect(game, "fireball", "Sounds/smb_fireball");
            soundManager.LoadEffect(game, "pause", "Sounds/smb_pause");
            soundManager.LoadEffect(game, "warning", "Sounds/smb_warning");
            soundManager.LoadEffect(game, "oneUp", "Sounds/smb_1-up");
        }
    }
}
