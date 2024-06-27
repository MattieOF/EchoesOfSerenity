using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.World.Item;

namespace EchoesOfSerenity.World.Tiles;

public static class Tiles
{
    public static Tileset TerrainTileset = null!;

    public static Tile Grass = new Tile { IsSolid = false, TileSetIndex = 0, RandomRotation = true };
    public static Tile FloweryGrass = new Tile { IsSolid = false, TileSetIndex = 10, RandomRotation = true };
    public static Tile Sand = new Tile { IsSolid = false, TileSetIndex = 1, RandomRotation = true };
    
    public static Tile Water = new Tile { IsSolid = false, TileSetIndex = 2, Animated = true, Frames = 6, FPS = 3};
    public static Tile DeepWater = new Tile { IsSolid = false, TileSetIndex = 18, Animated = true, Frames = 6, FPS = 3};

    public static Tile StoneFloor = new Tile { IsSolid = false, TileSetIndex = 8, RandomRotation = true };
    public static Tile StoneWall = new Tile { IsSolid = true, TileSetIndex = 9, RandomRotation = true, Strength = 5, RequiredTool = ToolType.Pickaxe, Drops = [(Items.Stone, 1, 3)] };
    public static Tile IronOre = new Tile { IsSolid = true, TileSetIndex = 24, RandomRotation = true, Strength = 10, RequiredTool = ToolType.Pickaxe, MinimumToolStrength = 3, Drops = [(Items.Stone, 1, 2), (Items.RawIron, 1, 2)] };
    public static Tile CoalOre = new Tile { IsSolid = true, TileSetIndex = 25, RandomRotation = true, Strength = 6, RequiredTool = ToolType.Pickaxe, Drops = [(Items.Stone, 1, 2), (Items.Coal, 1, 2)] };
    public static Tile Pebbles = new Tile { IsSolid = false, TileSetIndex = 11, RandomRotation = false, Strength = 3, CanBePunched = true, RequiredTool = ToolType.Pickaxe, Drops = [(Items.Pebbles, 1, 1)] };
    public static Tile Rock = new Tile { IsSolid = true, TileSetIndex = 12, RandomRotation = false, CanBePunched = false, RequiredTool = ToolType.Pickaxe, Strength = 4 };
    public static Tile Tree = new Tile { IsSolid = true, TileSetIndex = 13, RandomRotation = false, CanBePunched = true, RequiredTool = ToolType.Axe, Strength = 7, Drops = [(Items.Wood, 3, 6)] };
    public static Tile WoodPlank = new Tile { IsSolid = true, TileSetIndex = 14, RandomRotation = false, CanBePunched = true, RequiredTool = ToolType.Axe, Strength = 4, Drops = [(Items.WoodPlank, 1, 1)] };
    public static Tile WorkBench = new Tile { IsSolid = true, TileSetIndex = 15, RandomRotation = false, CanBePunched = true, RequiredTool = ToolType.Axe, Strength = 7, Drops = [(Items.WorkBench, 1, 1)], Name = "Work Bench" };
    public static Tile IronAnvil = new Tile { IsSolid = true, TileSetIndex = 16, RandomRotation = false, CanBePunched = false, RequiredTool = ToolType.Pickaxe, Strength = 7, Drops = [(Items.IronAnvil, 1, 1)], Name = "Iron Anvil" };
    public static Tile Furnace = new Tile { IsSolid = true, TileSetIndex = 17, RandomRotation = false, CanBePunched = false, RequiredTool = ToolType.Pickaxe, Strength = 7, Drops = [(Items.Furnace, 1, 1)], Name = "Furnace" };
    
    // TODO: Deep water, around of edge of map and where water gets deep. Kills player.

    public static void Init()
    {
        TerrainTileset = new Tileset("Content/Spritesheets/TerrainSpritesheet.png", 16, 16);
    }
}
