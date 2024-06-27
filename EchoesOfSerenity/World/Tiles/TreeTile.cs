using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.World.Entity;

namespace EchoesOfSerenity.World.Tiles;

public class TreeTile : Tile
{
    public override void OnBroken(Tilemap tilemap, int x, int y, Core.Entity.Entity? cause)
    {
        if (cause is PlayerEntity player)
        {
            player.Achievements.CompleteAchievement("punch_tree");
        }
    }
}
