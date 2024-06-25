using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Entity;
using EchoesOfSerenity.Core.Tilemap;

namespace EchoesOfSerenity.World;

public class World
{
    public Tilemap BaseLayer = null!;
    public Tilemap TopLayer = null!;
    public int Width, Height;
    public int Seed = 0;

    private List<Entity> _entities = new();
    private List<Entity> _queuedFrees = new();

    public static World CreateEmpty()
    {
        World world = new();
        world.BaseLayer = new Tilemap(Tilemap.ChunkSize, Tilemap.ChunkSize, Tiles.Tiles.TerrainTileset);
        world.TopLayer = new Tilemap(Tilemap.ChunkSize, Tilemap.ChunkSize, Tiles.Tiles.TerrainTileset);
        return world;
    }

    public void AddEntity(Entity entity)
    {
        entity.World = this;
        _entities.Add(entity);
    }

    public void RemoveEntity(Entity entity)
    {
        _queuedFrees.Add(entity);
    }

    public void Update()
    {
        foreach (var entity in _entities)
        {
            entity.Update();
        }
        
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

    public Vector2 GetCenterPoint()
    {
        return new((Width * TopLayer.Tileset.TileWidth) / 2.0f, (Height * TopLayer.Tileset.TileHeight) / 2.0f);
    }
}
