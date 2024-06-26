using System.Numerics;
using Raylib_cs;

namespace EchoesOfSerenity.World.Entity;

public class Bomb : Core.Entity.AnimatedEntity
{
    public Vector2 Velocity;
    public float fuse = 2;
    
    public Bomb()
    {
        Size = new(16, 16);
        Spritesheet = Spritesheets.Bomb;
        SetAnimation("blow");
    }

    public override void Update()
    {
        float delta = Raylib.GetFrameTime();

        if (Velocity != Vector2.Zero)
        {
            var prevPos = Position;
            Position += Velocity * delta;
            if (World.CheckCollision(BoundingBox))
            {
                Position = prevPos;
                Velocity = Vector2.Zero;
            }
            Velocity -= Velocity * delta;
        }
        
        fuse -= delta;
        if (fuse <= 0)
        {
            (int x, int y) = World.TopLayer.WorldCoordToTileCoord(Center);
            
            // Iterate over a 6x6 area around the bomb
            for (int cy = y - 3; cy < y + 3; cy++)
            {
                for (int cx = x - 3; cx < x + 3; cx++)
                {
                    World.TopLayer.DestroyTile(cx, cy, 7);
                }
            }
            
            World.ParticleSystem.AddParticle("Content/Spritesheets/ExplosionSpritesheet.png", Center, Vector2.Zero, 0.5f, new Rectangle(0, 0, 32, 32), 11, 32, 0.5f / 11.0f);
            World.RemoveEntity(this);
        }
    }
}
