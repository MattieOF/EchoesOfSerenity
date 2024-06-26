using System.Diagnostics;
using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Entity;
using EchoesOfSerenity.Core.Tilemap;
using Raylib_cs;

namespace EchoesOfSerenity.World;

public class World
{
    public Tilemap BaseLayer = null!;
    public Tilemap TopLayer = null!;
    public int Width, Height;
    public int Seed = 0;

    private List<Core.Entity.Entity> _entities = new();
    private List<Core.Entity.Entity> _queuedFrees = new();
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
        _entities.Add(entity);
    }

    public void RemoveEntity(Core.Entity.Entity entity)
    {
        _queuedFrees.Add(entity);
    }
    
    public void RemoveAllEntities()
    {
        Debug.Assert(_isUpdating is false);
        _entities.Clear();
    }

    public int GetEntityCount() => _entities.Count;

    public bool CheckCollision(Rectangle rectangle)
    {
        return TopLayer.CheckCollision(rectangle) || BaseLayer.CheckCollision(rectangle);
    }
    
    public void Update()
    {
        _isUpdating = true;
        foreach (var entity in _entities)
        {
            entity.Update();
        }
        _isUpdating = false;
        
        foreach (var entity in _queuedFrees)
            _entities.Remove(entity);
        _queuedFrees.Clear();
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
        
        foreach (var entity in _entities)
        {
            entity.Render();
        }
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
}
