using System.Diagnostics;
using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.World.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.World.Gen;

// TODO: Zoom in at start

public class WorldGen
{
    public static float IslandThreshold = 0.35f, IslandNoiseFrequency = 0.01f, IslandNoiseMix = 0.7f;
    public static float LakeThreshold = 0.2f, DeepLakeThreshold = 0.015f, SandThreshold = 0.2f;
    public static float MainNoiseFreq = 0.04f, MainNoise2Freq = 0.1f, MainNoise3Freq = 0.02f;
    public static float CaveNoiseFreq = 0.04f, CaveNoiseThreshold = 0.65f, CaveWallThickness = 0.12f;
    
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
        Stopwatch sw = new();
        sw.Start();
        
        if (seed == 0)
            seed = new Random().Next();
        Random rnd = new(seed);
        world.Seed = seed;
        
        world.RemoveAllEntities();
        
        world.TopLayer.Clear();
        int tilesX = world.Width, tilesY = world.Height;
        
        FastNoiseLite mainNoise = new(rnd.Next());
        mainNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        mainNoise.SetFrequency(MainNoiseFreq);
        FastNoiseLite mainNoise2 = new(rnd.Next());
        mainNoise2.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        mainNoise2.SetFrequency(MainNoise2Freq);
        FastNoiseLite mainNoise3 = new(rnd.Next());
        mainNoise3.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        mainNoise3.SetFrequency(MainNoise3Freq);
        FastNoiseLite islandNoise = new(rnd.Next());
        islandNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        islandNoise.SetFrequency(IslandNoiseFrequency);
        FastNoiseLite caveNoise = new(rnd.Next());
        caveNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        caveNoise.SetFrequency(CaveNoiseFreq);

        Vector2 centerPoint = new(tilesX / 2.0f, tilesY / 2.0f);
        float maxDist = Raymath.Vector2DistanceSqr(new Vector2(0, 0), centerPoint);
        
        for (int y = 0; y < tilesY; y++)
        {
            for (int x = 0; x < tilesX; x++)
            {
                float nx = 2.0f * x / world.Width - 1;
                float ny = 2.0f * y / world.Height - 1;
                float dist = 1 - (1 - (nx * nx)) * (1 - (ny * ny));
                float noise = islandNoise.GetNoise(x, y);

                float islandVal = Raymath.Lerp(noise, 1 - dist, IslandNoiseMix);
                bool isLand = islandVal > IslandThreshold;
                if (!isLand)
                {
                    world.BaseLayer.SetTile(x, y, dist > 0.85 ? Tiles.Tiles.DeepWater : Tiles.Tiles.Water);
                    continue;
                }

                float mainNoiseVal = mainNoise.GetNoise(x, y);
                
                if (islandVal < IslandThreshold + Math.Clamp(((mainNoiseVal + 0.6f) * 0.05f), 0, 1))
                {
                    world.BaseLayer.SetTile(x, y, Tiles.Tiles.Sand);
                    continue;
                }
                
                float mainNoise3Val = mainNoise3.GetNoise(x, y);
                float caveNoiseVal = caveNoise.GetNoise(x, y);

                void AddRocks()
                {
                    if (mainNoise3Val is > 0.6f and < 0.8f
                        && rnd.Next(0, 6) == 0)
                    {
                        world.TopLayer.SetTile(x, y, Tiles.Tiles.Pebbles);
                    } else if (mainNoise3Val is > 0.5f and < 0.55f
                            && rnd.Next(0, 10) == 0)
                    {
                        world.TopLayer.SetTile(x, y, Tiles.Tiles.Rock);
                    }
                }
                
                float absCaveNoiseVal = MathF.Abs(caveNoiseVal);
                if (absCaveNoiseVal < CaveNoiseThreshold)
                {
                    world.BaseLayer.SetTile(x, y, Tiles.Tiles.StoneFloor);

                    if (absCaveNoiseVal < CaveNoiseThreshold && absCaveNoiseVal > CaveNoiseThreshold - CaveWallThickness
                        || world.BaseLayer.TileTouches(x, y, Tiles.Tiles.Water)
                        || islandVal < IslandThreshold + 0.1f)
                    {
                        world.TopLayer.SetTile(x, y, Tiles.Tiles.StoneWall);
                    }
                    else
                    {
                        AddRocks();
                    }
                    
                    continue;
                }
                
                float mainNoise2Val = mainNoise2.GetNoise(x, y);
                if (mainNoiseVal > 0.3f && mainNoiseVal < 0.3f + LakeThreshold && mainNoise2Val is < 0.25f)
                {
                    bool isDeep = mainNoiseVal > 0.3 + (LakeThreshold / 2) - DeepLakeThreshold
                        && mainNoiseVal < 0.3 + (LakeThreshold / 2) + DeepLakeThreshold;
                    world.BaseLayer.SetTile(x, y, isDeep ? Tiles.Tiles.DeepWater : Tiles.Tiles.Water);
                    continue;
                }
                
                if (mainNoiseVal > 0.3f + LakeThreshold && mainNoiseVal < 0.3f + LakeThreshold + SandThreshold
                                                             && mainNoise2Val < SandThreshold)
                {
                    world.BaseLayer.SetTile(x, y, Tiles.Tiles.Sand);
                    continue;
                }

                if (mainNoise2Val is > 0.4f and < 0.6f
                    && rnd.Next(0, 3) == 0)
                {
                    world.BaseLayer.SetTile(x, y, Tiles.Tiles.FloweryGrass);
                } 
                else
                    world.BaseLayer.SetTile(x, y, Tiles.Tiles.Grass); 
                
                AddRocks();
            }
        }
        
        // Spawn player
        int tries = 30;
        int playerX = rnd.Next((int)(centerPoint.X - 128), (int)(centerPoint.X + 128)), playerY = rnd.Next((int)(centerPoint.Y - 128), (int)(centerPoint.Y + 128));
        while (!IsTileValidSpawn(world, playerX, playerY) && tries-- > 0)
        {
            playerX = rnd.Next((int)(centerPoint.X - 128), (int)(centerPoint.X + 128));
            playerY = rnd.Next((int)(centerPoint.Y - 128), (int)(centerPoint.Y + 128));
        }
        PlayerEntity player = new();
        player.Center = new(playerX * Tiles.Tiles.TerrainTileset.TileWidth, playerY * Tiles.Tiles.TerrainTileset.TileHeight);
        Game.Instance.SetCameraTarget(player.Center);
        world.AddEntity(player);
        
        sw.Stop();
        Utility.WriteLineColour(ConsoleColor.Green, $"Took {sw.Elapsed.TotalMilliseconds:F}ms to generate level");
        
        world.BaseLayer.RerenderAll();
        world.TopLayer.RerenderAll();
    }

    public static bool IsTileValidSpawn(World world, int x, int y)
    {
        var top = world.TopLayer.TileAt(x, y);
        if (top is not null && top.IsSolid)
            return false;
        
        var baseTile = world.BaseLayer.TileAt(x, y);
        if (baseTile is null || baseTile.IsSolid || baseTile == Tiles.Tiles.Water || baseTile == Tiles.Tiles.DeepWater || baseTile == Tiles.Tiles.StoneFloor)
            return false;
        
        return true;
    }
}
