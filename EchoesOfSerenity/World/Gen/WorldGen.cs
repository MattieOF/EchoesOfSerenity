using EchoesOfSerenity.Core.Tilemap;

namespace EchoesOfSerenity.World.Gen;

public class WorldGen
{
    public static World GenerateWorld(int chunkCountX, int chunkCountY, int seed = 0)
    {
        if (seed == 0)
            seed = new Random().Next();
        Random rnd = new(seed);
        
        World world = new()
        {
            Seed = seed
        };

        int tilesX = chunkCountX * Tilemap.ChunkSize, tilesY = chunkCountY * Tilemap.ChunkSize;
        
        world.BaseLayer = new Tilemap(tilesX, tilesY, Tiles.Tiles.TerrainTileset);
        world.TopLayer = new Tilemap(tilesX, tilesY, Tiles.Tiles.TerrainTileset);
        world.Width = tilesX;
        world.Height = tilesY;

        for (int y = 0; y < tilesY; y++)
        {
            for (int x = 0; x < tilesX; x++)
            {
                if (x < 32 || x > tilesX - 32 
                    || y < 32 || y > tilesY - 32)
                {
                    world.BaseLayer.SetTile(x, y, Tiles.Tiles.Water);
                    continue;
                }
                
                // world.BaseLayer.SetTile(x, y, Tiles.Tiles.Grass);
                world.BaseLayer.SetTile(x, y, rnd.Next(0, 10) == 0 ? Tiles.Tiles.Water : Tiles.Tiles.Grass);
            }
        }
        
        return world;
    }
}
