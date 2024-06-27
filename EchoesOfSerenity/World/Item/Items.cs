using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.World.Entity;

namespace EchoesOfSerenity.World.Item;

public static class Items
{
    public static Item TestItem1 = new Item
    {
        Name = "Test Item 1"
    };

    public static Item Wood = new Item
    {
        Name = "Wood",
        Texture = ContentManager.GetTexture("Content/Items/Wood.png"),
    };
    
    public static Item JakeVoodooDoll = new JakeVoodooDollItem();
    public static Item Bomb = new BombItem();

    public static TileItem Pebbles = new TileItem()
    {
        Name = "Pebbles",
        Texture = ContentManager.GetTexture("Content/Items/Pebbles.png"),
    };

    public static void PostTileInit()
    {
        Pebbles.Tile = Tiles.Tiles.Pebbles;
    }
}
