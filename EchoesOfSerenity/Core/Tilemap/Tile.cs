using EchoesOfSerenity.World.Item;
using Raylib_cs;

namespace EchoesOfSerenity.Core.Tilemap;

public class Tile
{
    public string Name = "";
    
    public bool IsSolid = false;
    public bool HasBorder = false;
    public int TileSetIndex = 0;
    public bool RandomRotation = false;

    public bool Animated = false;
    public int Frames = 1; // Should be stored sequentially in the tileset, in the X direction
    public int FPS = 3;

    public bool Replaceable = false;
    public int Strength = 5, MinimumToolStrength = 0;
    public ToolType RequiredTool = ToolType.None;
    public bool CanBePunched = false;
    public List<(Item, int, int)> Drops = [];

    public Sound HitSound, BreakSound;

    public virtual void OnBroken(Tilemap tilemap, int x, int y, Entity.Entity? cause)
    {
    }
}
