using EchoesOfSerenity.Core.Tilemap;

namespace EchoesOfSerenity.World.Gen;

public class WorldGen
{
    public World GenerateWorld(int chunkCountX, int chunkCountY)
    {
        World world = new();
        
        int tilesX = chunkCountX * Tilemap.ChunkSize, tilesY = chunkCountY * Tilemap.ChunkSize;
        
        world.BaseLayer = new Tilemap(tilesX, tilesY, Tiles.Tiles.TerrainTileset);
        world.TopLayer = new Tilemap(tilesX, tilesY, Tiles.Tiles.TerrainTileset);

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
                
                world.BaseLayer.SetTile(x, y, Tiles.Tiles.Grass);
            }
        }
        
        return world;
    }
}
