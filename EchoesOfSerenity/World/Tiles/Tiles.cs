using EchoesOfSerenity.Core.Tilemap;

namespace EchoesOfSerenity.World.Tiles;

public static class Tiles
{
    public static Tileset TerrainTileset = null!;

    public static Tile Grass = new Tile { IsSolid = false, TileSetIndex = 0 };
    public static Tile Sand = new Tile { IsSolid = false, TileSetIndex = 1 };
    public static Tile Water = new Tile { IsSolid = false, TileSetIndex = 2, Animated = true, Frames = 6, FPS = 3};
    public static Tile DeepWater = new Tile { IsSolid = false, TileSetIndex = 18, Animated = true, Frames = 6, FPS = 3};

    public static Tile StoneFloor = new Tile { IsSolid = false, TileSetIndex = 8 };
    public static Tile StoneWall = new Tile { IsSolid = true, TileSetIndex = 9 };
    public static Tile FloweryGrass = new Tile { IsSolid = false, TileSetIndex = 10, RandomRotation = true };
    public static Tile Pebbles = new Tile { IsSolid = false, TileSetIndex = 11, RandomRotation = false };
    public static Tile Rock = new Tile { IsSolid = true, TileSetIndex = 12, RandomRotation = false };
    
    // TODO: Deep water, around of edge of map and where water gets deep. Kills player.

    public static void Init()
    {
        TerrainTileset = new Tileset("Content/Spritesheets/TerrainSpritesheet.png", 16, 16);
    }
}
