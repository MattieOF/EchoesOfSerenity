using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Entity;
using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.World.Item;
using Raylib_cs;

namespace EchoesOfSerenity.World.Entity;

public class TileItem : Item.Item
{
    private readonly Tile _tile;

    public TileItem(Tile tile)
    {
        UseType = UseType.Placeable;
        _tile = tile;
    }

    public override bool OnUsed(LivingEntity user)
    {
        if (user is PlayerEntity player)
        {
            var targetedTile = Game.Instance.ScreenPosToWorld(Raylib.GetMousePosition());
            if (Raymath.Vector2DistanceSqr(player.Center, targetedTile) < player.PlaceRange * player.PlaceRange)
            {
                Tile? tileAt = player.World.TopLayer.TileAtWorldCoord(targetedTile);
                if (tileAt is null || tileAt.Replaceable)
                {
                    player.World.TopLayer.SetTile((int)MathF.Floor(targetedTile.X), (int)MathF.Floor(targetedTile.Y), _tile);
                    return true;
                }
            }
            return true;
        }
        
        return false;
    }
}
