using Raylib_cs;

namespace EchoesOfSerenity.World.Achievement;

public class Achievement
{
    public string Name = "";
    public string Description = "";
    public bool Completed = false;
    public string StatID = "";
    public float StatGoal = 0;
    public Texture2D Icon;
}
