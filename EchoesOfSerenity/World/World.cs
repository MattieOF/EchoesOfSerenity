using System.Numerics;
using EchoesOfSerenity.Core.Tilemap;

namespace EchoesOfSerenity.World;

public class World
{
    public Tilemap BaseLayer = null!;
    public Tilemap TopLayer = null!;
    public int Width, Height;
    public int Seed = 0;

    public static World CreateEmpty()
    {
        World world = new();
        world.BaseLayer = new Tilemap(Tilemap.ChunkSize, Tilemap.ChunkSize, Tiles.Tiles.TerrainTileset);
        world.TopLayer = new Tilemap(Tilemap.ChunkSize, Tilemap.ChunkSize, Tiles.Tiles.TerrainTileset);
        return world;
    }

    public void PreRender()
    {
        BaseLayer.PreRender();
        TopLayer.PreRender();
    }

    public void Render()
    {
        BaseLayer.Render();
        TopLayer.Render();
    }

    public Vector2 GetCenterPoint()
    {
        return new((Width * TopLayer.Tileset.TileWidth) / 2.0f, (Height * TopLayer.Tileset.TileHeight) / 2.0f);
    }
}
