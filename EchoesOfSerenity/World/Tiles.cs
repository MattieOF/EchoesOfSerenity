using EchoesOfSerenity.Core.Tilemap;

namespace EchoesOfSerenity.World;

public static class Tiles
{
    public static Tileset TerrainTileset = null!;

    public static Tile Grass = new Tile { IsSolid = false, TileSetIndex = 0, RandomRotation = true };
    public static Tile Sand = new Tile { IsSolid = false, TileSetIndex = 1 };
    public static Tile Water = new Tile { IsSolid = true, TileSetIndex = 2 };

    public static void Init()
    {
        TerrainTileset = new Tileset("Content/Spritesheets/TerrainSpritesheet.png", 16, 16);
    }
}
