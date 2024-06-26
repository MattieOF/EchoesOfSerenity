using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.World.Entity;

public class PlayerEntity : Mob
{
    public float MoveSpeed = 96;
    public float SpeedMultiplier = 1;
    private Vector2 _lastMovement, _lastLerpedMovement;

    public static float IntroAnimInitialZoom = 0.3f, IntroAnimTargetZoom = 1.4f, IntroAnimZoomSpeed = 0.4f, IntroAnimZoomDelay = 2.5f;
    private float _introAnimTimer = 0;
    private bool _introAnimActive = true;
    
    public PlayerEntity()
    {
        Size = new(15, 15);
        Spritesheet = Spritesheets.Player;
        SetAnimation("idle");
        _introAnimTimer = IntroAnimZoomDelay;
        Game.Instance.CameraZoom = IntroAnimInitialZoom;
        Echoes.EchoesInstance.HUD.Player = this;
    }

    public override void Update()
    {
        // Check if we're in water
        bool inWater = World.BaseLayer.TileAtWorldCoord(Center) == Tiles.Tiles.Water;
        if (inWater)
        {
            SetAnimation("in_water");
            SpeedMultiplier = 0.3f;
        }
        else SpeedMultiplier = 1;
        
        if (_introAnimActive)
        {
            float howClose = MathF.Abs(Game.Instance.CameraZoom - IntroAnimTargetZoom);
            if (_introAnimTimer > 0)
            {
                _introAnimTimer -= Raylib.GetFrameTime();
            }
            else
            {
                Game.Instance.CameraZoom = Utility.LerpSmooth(Game.Instance.CameraZoom, IntroAnimTargetZoom, IntroAnimZoomSpeed);
                if (howClose < 0.01f)
                {
                    Game.Instance.CameraZoom = IntroAnimTargetZoom;
                    _introAnimActive = false;
                }
            }

            if (howClose > 0.25f)
                return;
        }
        
        Vector2 movement = Vector2.Zero;
        if (Raylib.IsKeyDown(KeyboardKey.W))
            movement.Y -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.S))
            movement.Y += 1;
        if (Raylib.IsKeyDown(KeyboardKey.A))
            movement.X -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.D))
            movement.X += 1;
        
        if (movement != Vector2.Zero)
        {
            movement = Vector2.Normalize(movement) * (MoveSpeed * SpeedMultiplier * Raylib.GetFrameTime());
            _lastMovement = movement;

            var newPos = Position;
            newPos.X += movement.X;
            if (World.CheckCollision(new Rectangle(newPos.X, newPos.Y, Size.X, Size.Y)))
                newPos.X = Position.X;
            newPos.Y += movement.Y;
            if (World.CheckCollision(new Rectangle(newPos.X, newPos.Y, Size.X, Size.Y)))
                newPos.Y = Position.Y;
            Position = newPos;

            if (!inWater) SetAnimation("walk");
        }
        else
        {
            if (!inWater) SetAnimation("idle");
        }
        
        Vector2 lerpedMovement = Utility.LerpSmooth(_lastLerpedMovement, _lastMovement, 0.02f);
        _lastLerpedMovement = lerpedMovement;
        Rot = (float)Math.Atan2(lerpedMovement.Y, lerpedMovement.X) * (180 / MathF.PI);
        
        Game.Instance.CameraTarget = Center;
    }
}
