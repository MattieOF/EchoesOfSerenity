using EchoesOfSerenity.Core.Content;

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
}
