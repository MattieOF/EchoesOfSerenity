using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Entity;
using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.World.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.World.Item;

public class TileItem : Item
{
    public Tile Tile = null!;

    public TileItem()
    {
        UseType = UseType.Placeable;
        UseSpeed = 0;
    }

    public override bool OnAltUsed(LivingEntity user)
    {
        if (user is PlayerEntity player)
        {
            var targetedTile = Game.Instance.ScreenPosToWorld(Raylib.GetMousePosition());
            if (Raymath.Vector2DistanceSqr(player.Center, targetedTile) < player.PlaceRange * player.PlaceRange)
            {
                Tile? tileAt = player.World.TopLayer.TileAtWorldCoord(targetedTile);
                if ((tileAt is null || tileAt.Replaceable) && tileAt != Tile)
                {
                    player.World.TopLayer.SetTileAtWorldCoord((int)MathF.Floor(targetedTile.X), (int)MathF.Floor(targetedTile.Y), Tile);
                    player.Inventory.Contents[player.SelectedHotbarSlot] = (this, player.Inventory.Contents[player.SelectedHotbarSlot].Item2 - 1);
                    if (player.Inventory.Contents[player.SelectedHotbarSlot].Item2 == 0)
                        player.Inventory.Contents[player.SelectedHotbarSlot] = (null, 0);
                    return true;
                }
            }
        }
        
        return false;
    }
}
