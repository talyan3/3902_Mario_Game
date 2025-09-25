namespace MonogameTest;

// As of 9/25 the only use of this class is to store the active sprite of Mario
// For future sprints, this may be used to store the state of mario as well (big, small, fire, star power up, etc.)
public class MarioManager
{
    public ISprite ActiveSprite { get; set; }
    public MarioManager()
    {

    }
}