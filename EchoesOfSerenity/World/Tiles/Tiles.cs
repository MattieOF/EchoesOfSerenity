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
    public static Tile StoneWall = new Tile { IsSolid = true, TileSetIndex = 9, RandomRotation = true, Strength = 5, RequiredTool = ToolType.Pickaxe, Drops = [(Items.Bomb, 1, 2)] };
    public static Tile Pebbles = new Tile { IsSolid = false, TileSetIndex = 11, RandomRotation = false, Strength = 3, CanBePunched = true, RequiredTool = ToolType.Pickaxe, Drops = [(Items.Pebbles, 1, 1)] };
    public static Tile Rock = new Tile { IsSolid = true, TileSetIndex = 12, RandomRotation = false, CanBePunched = false, RequiredTool = ToolType.Pickaxe, Strength = 4 };
    public static Tile Tree = new Tile { IsSolid = true, TileSetIndex = 13, RandomRotation = false, CanBePunched = true, RequiredTool = ToolType.Axe, Strength = 7, Drops = [(Items.Wood, 3, 6)] };
    
    // TODO: Deep water, around of edge of map and where water gets deep. Kills player.

    public static void Init()
    {
        TerrainTileset = new Tileset("Content/Spritesheets/TerrainSpritesheet.png", 16, 16);
    }
}
