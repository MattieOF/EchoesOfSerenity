using System.Numerics;
using EchoesOfSerenity.Core.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.World.Entity;

public class BombEntity : Core.Entity.AnimatedEntity
{
    public Vector2 Velocity;
    private float _fuse = 2;
    private Core.Entity.Entity? _source;
    
    public BombEntity(Core.Entity.Entity? source)
    {
        _source = source;
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
        
        _fuse -= delta;
        if (_fuse <= 0)
        {
            (int x, int y) = World.TopLayer.WorldCoordToTileCoord(Center);
            
            // Iterate over a 6x6 area around the bomb
            int destroyedTiles = 0;
            for (int cy = y - 3; cy < y + 3; cy++)
            {
                for (int cx = x - 3; cx < x + 3; cx++)
                {
                    if (World.TopLayer.TileAtTileCoord(cx, cy) is not null)
                        destroyedTiles++;
                        
                    World.TopLayer.DestroyTile(cx, cy, 7, _source);
                }
            }
            
            if (_source is PlayerEntity player)
                player.Stats.AddStat("tiles_blown_up", destroyedTiles);

            float explosionRadius = 64 * 64;
            foreach (var entity in World.Entities)
            {
                if (entity is LivingEntity livingEntity)
                {
                    var dist = Vector2.DistanceSquared(Center, livingEntity.Center);
                    if (dist < explosionRadius)
                    {
                        livingEntity.Hurt(Raymath.Lerp(12, 0, Math.Clamp(dist / explosionRadius, 0, 1)));
                    }
                }
            }
            
            World.ParticleSystem.AddParticle("Content/Spritesheets/ExplosionSpritesheet.png", Center - Vector2.One * 16, Vector2.Zero, 0.5f, new Rectangle(0, 0, 32, 32), 11, 32, 0.5f / 11.0f, false);
            World.RemoveEntity(this);
        }
    }
}
