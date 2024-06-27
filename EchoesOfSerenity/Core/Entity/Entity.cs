using System.Numerics;
using Raylib_cs;

namespace EchoesOfSerenity.Core.Entity;

public class Entity
{
    public Vector2 Position;
    public Vector2 Size;

    public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
    
    public static bool DrawBoundingBoxes = false;

    public Vector2 Center
    {
        get => new(Position.X + Size.X / 2, Position.Y + Size.Y / 2);
        set => Position = new Vector2(value.X - Size.X / 2, value.Y - Size.Y / 2);
    }

    public World.World World = null!;
    
    public virtual void OnAddedToWorld()
    { }
    
    public virtual void Update()
    { }

    public virtual void Render()
    {
        if (DrawBoundingBoxes)
            Raylib.DrawRectangleLinesEx(BoundingBox, 1, Color.Red);
    }
    
    public virtual void RenderUI()
    { }

    public virtual void Kill()
    {
        World.RemoveEntity(this);
    }
}
