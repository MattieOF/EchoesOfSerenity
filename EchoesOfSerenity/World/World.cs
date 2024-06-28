using System.Diagnostics;
using System.Numerics;
using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.World.Entity;
using EchoesOfSerenity.World.Particle;
using Raylib_cs;

namespace EchoesOfSerenity.World;

public class World : IDisposable
{
    public Tilemap BaseLayer = null!;
    public Tilemap TopLayer = null!;
    public ParticleSystemLayer ParticleSystem = new();
    public int Width, Height;
    public int Seed = 0;
    public float Time = 0;
    public Vector2 SpawnPoint;
    public PlayerEntity Player = null!;

    public List<Core.Entity.Entity> Entities { get; } = new();
    
    private List<Core.Entity.Entity> _queuedFrees = new();
    private List<Core.Entity.Entity> _queuedAdds = new();
    private bool _isUpdating = false;

    public static World CreateEmpty()
    {
        World world = new();
        world.BaseLayer = new Tilemap(Tilemap.ChunkSize, Tilemap.ChunkSize, Tiles.Tiles.TerrainTileset);
        world.TopLayer = new Tilemap(Tilemap.ChunkSize, Tilemap.ChunkSize, Tiles.Tiles.TerrainTileset);
        return world;
    }
    
    public void AddEntity(Core.Entity.Entity entity)
    {
        entity.World = this;
        
        if (_isUpdating)
            _queuedAdds.Add(entity);
        else
        {
            Entities.Add(entity);
            entity.OnAddedToWorld();
        }
    }

    public void RemoveEntity(Core.Entity.Entity entity)
    {
        _queuedFrees.Add(entity);
    }
    
    public void RemoveAllEntities()
    {
        Debug.Assert(_isUpdating is false);
        Entities.Clear();
    }

    public int GetEntityCount() => Entities.Count;

    public bool CheckCollision(Rectangle rectangle)
    {
        return TopLayer.CheckCollision(rectangle) || BaseLayer.CheckCollision(rectangle);
    }
    
    public void Update()
    {
        Time += Raylib.GetFrameTime();
        
        _isUpdating = true;
        foreach (var entity in Entities)
        {
            entity.Update();
        }
        _isUpdating = false;
        
        foreach (var entity in _queuedFrees)
            Entities.Remove(entity);
        _queuedFrees.Clear();

        foreach (var entity in _queuedAdds)
        {
            Entities.Add(entity);
            entity.OnAddedToWorld();
        }
        _queuedAdds.Clear();
    }
    
    public void PreRender()
    {
        BaseLayer.PreRender();
        TopLayer.PreRender();
    }

    public void Render()
    {
        BaseLayer.Render();
        ParticleSystem.Render();
        TopLayer.Render();
        
        foreach (var entity in Entities)
            entity.Render();
    }

    public void RenderUI()
    {
        foreach (var entity in Entities)
            entity.RenderUI();
    }

    public void RerenderAll()
    {
        BaseLayer.RerenderAll();
        TopLayer.RerenderAll();
    }

    public Vector2 GetCenterPoint()
    {
        return new((Width * TopLayer.Tileset.TileWidth) / 2.0f, (Height * TopLayer.Tileset.TileHeight) / 2.0f);
    }

    public void Dispose()
    {
        BaseLayer.Dispose();
        TopLayer.Dispose();
    }

    public bool IsTileWithinRange(Tile tile, Vector2 Origin, int Range)
    {
        (int x, int y) = TopLayer.WorldCoordToTileCoord(Origin);
        for (int cy = y - Range; cy < y + Range; cy++)
        {
            for (int cx = x - Range; cx < x + Range; cx++)
            {
                if (TopLayer.TileAtTileCoord(cx, cy) == tile || BaseLayer.TileAtTileCoord(cx, cy) == tile)
                    return true;
            }
        }

        return false;
    }
}
