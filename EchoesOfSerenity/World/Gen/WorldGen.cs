using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Tilemap;
using Raylib_cs;

namespace EchoesOfSerenity.World.Gen;

public class WorldGen
{
    public static float islandThreshold = 0.35f, islandNoiseFrequency = 0.01f, islandNoiseMix = 0.7f;
    
    public static World GenerateWorld(int chunkCountX, int chunkCountY, int seed = 0)
    {
        World world = new();

        int tilesX = chunkCountX * Tilemap.ChunkSize, tilesY = chunkCountY * Tilemap.ChunkSize;
        world.BaseLayer = new Tilemap(tilesX, tilesY, Tiles.Tiles.TerrainTileset);
        world.TopLayer = new Tilemap(tilesX, tilesY, Tiles.Tiles.TerrainTileset);
        world.Width = tilesX;
        world.Height = tilesY;

        RegenerateWorld(world, seed);
        
        return world;
    }

    public static void RegenerateWorld(World world, int seed = 0)
    {
        if (seed == 0)
            seed = new Random().Next();
        Random rnd = new(seed);
        world.Seed = seed;
        
        int tilesX = world.Width, tilesY = world.Height;
        
        FastNoiseLite mainNoise = new(rnd.Next());
        FastNoiseLite islandNoise = new(rnd.Next());
        islandNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        islandNoise.SetFrequency(islandNoiseFrequency);

        Vector2 centerPoint = new(tilesX / 2.0f, tilesY / 2.0f);
        float maxDist = Raymath.Vector2DistanceSqr(new Vector2(0, 0), centerPoint);
        
        for (int y = 0; y < tilesY; y++)
        {
            for (int x = 0; x < tilesX; x++)
            {
                // if (x < 32 || x > tilesX - 32 
                //            || y < 32 || y > tilesY - 32)
                // {
                //     world.BaseLayer.SetTile(x, y, Tiles.Tiles.Water);
                //     continue;
                // }

                Vector2 location = new(x, y);
                float distSquared = Raymath.Vector2DistanceSqr(centerPoint, location);

                float nx = 2.0f * x / world.Width - 1;
                float ny = 2.0f * y / world.Height - 1;
                float dist = 1 - (1 - (nx * nx)) * (1 - (ny * ny));
                float noise = islandNoise.GetNoise(x, y);
                
                bool isLand = Raymath.Lerp(noise, 1 - dist, islandNoiseMix) > islandThreshold;
                // bool isLand = noise > 0.3f;
                // bool isLand = 1 - dist > 0.3f;
                if (!isLand)
                {
                    world.BaseLayer.SetTile(x, y, Tiles.Tiles.Water);
                    continue;
                }
                
                world.BaseLayer.SetTile(x, y, Tiles.Tiles.Grass);
            }
        }
    }
}
