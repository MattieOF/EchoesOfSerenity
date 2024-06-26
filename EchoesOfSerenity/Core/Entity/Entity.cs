using System.Numerics;
using Raylib_cs;

namespace EchoesOfSerenity.Core.Entity;

public class Entity
{
    public Vector2 Position;
    public Vector2 Size;

    public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

    public World.World World = null!;
    
    public virtual void Update()
    { }
    
    public virtual void Render()
    { }

    public virtual void Kill()
    {
        World.RemoveEntity(this);
    }
}
