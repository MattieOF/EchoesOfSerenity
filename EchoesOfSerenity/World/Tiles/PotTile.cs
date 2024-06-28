using System.Numerics;
using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.World.Entity;
using EchoesOfSerenity.World.Item;
using Raylib_cs;

namespace EchoesOfSerenity.World.Tiles;

public class PotTile : Tile
{
    public static List<(Item.Item item, int min, int max)> PossibleDrops =
    [
        (Items.IronIngot, 1, 2),
        (Items.SulfurDust, 3, 6),
        (Items.RawIron, 3, 6),
        (Items.Coal, 8, 12),
        (Items.Stone, 20, 30),
        (Items.CaveBread, 2, 5),
    ];
    
    public override void OnBroken(Tilemap tilemap, int x, int y, Core.Entity.Entity? cause)
    {
        if (cause is not null)
        {
            var drop = PossibleDrops[Raylib.GetRandomValue(0, PossibleDrops.Count - 1)];
            var entity = new ItemEntity(drop.item, Raylib.GetRandomValue(drop.min, drop.max))
            {
                Center = new Vector2((x * tilemap.Tileset.TileWidth) + tilemap.Tileset.TileWidth / 2.0f, (y * tilemap.Tileset.TileHeight) + tilemap.Tileset.TileHeight / 2.0f)
            };
            Echoes.EchoesInstance.World.AddEntity(entity);
        }
    }
}
