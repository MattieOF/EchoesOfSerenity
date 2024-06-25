using EchoesOfSerenity.Core.Tilemap;

namespace EchoesOfSerenity.World.Tiles;

public static class Tiles
{
    public static Tileset TerrainTileset = null!;

    public static Tile Grass = new Tile { IsSolid = false, TileSetIndex = 0, RandomRotation = true };
    public static Tile Sand = new Tile { IsSolid = false, TileSetIndex = 1 };
    public static Tile Water = new Tile { IsSolid = false, TileSetIndex = 2, Animated = true, Frames = 3 };
    public static Tile DeepWater = new Tile { IsSolid = false };
    
    // TODO: Deep water, around of edge of map and where water gets deep. Kills player.

    public static void Init()
    {
        TerrainTileset = new Tileset("Content/Spritesheets/TerrainSpritesheet.png", 16, 16);
    }
}
