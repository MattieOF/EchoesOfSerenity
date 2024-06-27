using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.World.Entity;

namespace EchoesOfSerenity.World.Item;

public static class Items
{
    public static Item Wood = new Item
    {
        Name = "Wood",
        Texture = ContentManager.GetTexture("Content/Items/Wood.png"),
    };

    public static Item JakeVoodooDoll = new JakeVoodooDollItem()
    {
        OnCrafted = [
            crafter =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("craft_jake");
            } ]
    };

    public static Item Bomb = new BombItem()
    {
        OnCrafted =
        [
            crafter =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("craft_bomb");
            }
        ]
    };

    public static TileItem Pebbles = new TileItem()
    {
        Name = "Pebbles",
        Texture = ContentManager.GetTexture("Content/Items/Pebbles.png"),
        OnPickedUp = [
            (crafter, _) =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("obtain_pebbles");
            } ]
    };
    
    public static TileItem WoodPlank = new TileItem()
    {
        Name = "Wooden Planks",
        Texture = ContentManager.GetTexture("Content/Items/WoodPlank.png"),
        Description = "Can be placed"
    };

    public static TileItem WorkBench = new TileItem()
    {
        Name = "Work Bench",
        Texture = ContentManager.GetTexture("Content/Items/WorkBench.png"),
        Description = "Used to make more interesting things!",
        OnCrafted = [
            crafter =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("craft_work_bench");
            } ]
    };

    public static TileItem IronAnvil = new TileItem()
    {
        Name = "Iron Anvil",
        Texture = ContentManager.GetTexture("Content/Items/IronAnvil.png"),
        Description = "Used to make more interesting things, with metal!",
        OnCrafted = [
            crafter =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("craft_anvil");
            } ]
    };

    public static TileItem Furnace = new TileItem()
    {
        Name = "Furnace",
        Texture = ContentManager.GetTexture("Content/Items/Furnace.png"),
        Description = "Used to make things hotter!",
        OnCrafted = [
            crafter =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("craft_furnace");
            } ]
    };

    public static Item Stick = new Item()
    {
        Name = "Stick",
        Texture = ContentManager.GetTexture("Content/Items/Stick.png"),
        Description = "No good at hitting things"
    };

    public static Item Stone = new Item()
    {
        Name = "Stone",
        Texture = ContentManager.GetTexture("Content/Items/Stone.png"),
        Description = "Used to make stone tools, among other things"
    };
    
    public static Item Coal = new Item()
    {
        Name = "Lump of Coal",
        Texture = ContentManager.GetTexture("Content/Items/Coal.png"),
        Description = "Rock make fire",
        OnPickedUp = [
            (crafter, _) =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("obtain_coal");
            } ]
    };
    
    public static Item RawIron = new Item()
    {
        Name = "Raw Iron",
        Texture = ContentManager.GetTexture("Content/Items/RawIron.png"),
        Description = "Can be smelted into something more useful",
        OnCrafted = [
            crafter =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("mine_iron");
            } ]
    };
    
    public static Item IronIngot = new Item()
    {
        Name = "Iron Ingot",
        Texture = ContentManager.GetTexture("Content/Items/IronIngot.png"),
        Description = "Used to make iron tools, among other things",
        OnCrafted = [
            crafter =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("smelt_iron");
            } ]
    };

    public static Item Flint = new Item()
    {
        Name = "Flint",
        Texture = ContentManager.GetTexture("Content/Items/Flint.png"),
    };

    public static Item WoodenPickaxe = new Item()
    {
        Name = "Wooden Pickaxe",
        Texture = ContentManager.GetTexture("Content/Items/WoodenPickaxe.png"),
        Description = "Can mine stone and coal",
        ToolType = ToolType.Pickaxe,
        ToolStrength = 1,
        UseType = UseType.Tool,
        MaxStack = 1,
        UseSpeed = .5f,
        OnCrafted = [
            crafter =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("craft_wood_tool");
            } ]
    };

    public static Item WoodenAxe = new Item()
    {
        Name = "Wooden Axe",
        Texture = ContentManager.GetTexture("Content/Items/WoodenAxe.png"),
        Description = "Mines wooden tiles faster",
        ToolType = ToolType.Axe,
        ToolStrength = 2,
        UseType = UseType.Tool,
        MaxStack = 1,
        UseSpeed = .5f,
        OnCrafted = [
            crafter =>
            {
                if (crafter is PlayerEntity player)
                    player.Achievements.CompleteAchievement("craft_wood_tool");
            } ]
    };
    
    public static Item StonePickaxe = new Item()
    {
        Name = "Stone Pickaxe",
        Texture = ContentManager.GetTexture("Content/Items/StonePickaxe.png"),
        Description = "Can mine iron",
        ToolType = ToolType.Pickaxe,
        ToolStrength = 3,
        UseType = UseType.Tool,
        MaxStack = 1,
        UseSpeed = .3f,
    };

    public static Item StoneAxe = new Item()
    {
        Name = "Stone Axe",
        Texture = ContentManager.GetTexture("Content/Items/StoneAxe.png"),
        ToolType = ToolType.Axe,
        ToolStrength = 3,
        UseType = UseType.Tool,
        MaxStack = 1,
        UseSpeed = .3f,
    };
    
    public static Item IronPickaxe = new Item()
    {
        Name = "Iron Pickaxe",
        Texture = ContentManager.GetTexture("Content/Items/IronPickaxe.png"),
        ToolType = ToolType.Pickaxe,
        ToolStrength = 6,
        UseType = UseType.Tool,
        MaxStack = 1,
        UseSpeed = .25f,
    };

    public static Item IronAxe = new Item()
    {
        Name = "Iron Axe",
        Texture = ContentManager.GetTexture("Content/Items/IronAxe.png"),
        ToolType = ToolType.Axe,
        ToolStrength = 6,
        UseType = UseType.Tool,
        MaxStack = 1,
        UseSpeed = .25f,
    };

    public static void PostTileInit()
    {
        Pebbles.Tile = Tiles.Tiles.Pebbles;
        WoodPlank.Tile = Tiles.Tiles.WoodPlank;
        WorkBench.Tile = Tiles.Tiles.WorkBench;
        IronAnvil.Tile = Tiles.Tiles.IronAnvil;
        Furnace.Tile = Tiles.Tiles.Furnace;
    }
}
