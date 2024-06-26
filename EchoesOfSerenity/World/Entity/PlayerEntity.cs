using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Entity;
using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.UI;
using EchoesOfSerenity.UI.Menus;
using EchoesOfSerenity.World.Item;
using Raylib_cs;

namespace EchoesOfSerenity.World.Entity;

public class PlayerEntity : LivingEntity
{
    public float MoveSpeed = 96;
    public float SpeedMultiplier = 1;
    public int SelectedHotbarSlot = 0;
    public Inventory Inventory = new(18);
    private Vector2 _lastMovement, _lastLerpedMovement;

    public static float IntroAnimInitialZoom = 0.3f, IntroAnimTargetZoom = 1.4f, IntroAnimZoomSpeed = 0.4f, IntroAnimZoomDelay = 2.5f;
    private float _introAnimTimer = 0;
    private bool _introAnimActive = true;
    private bool _drowned = false;
    
    public PlayerEntity()
    {
        Size = new(15, 15);
        Spritesheet = Spritesheets.Player;
        SetAnimation("idle");
        _introAnimTimer = IntroAnimZoomDelay;
        Game.Instance.CameraZoom = IntroAnimInitialZoom;
        Echoes.EchoesInstance.HUD!.Player = this;

        Inventory.Contents[0] = (Items.TestItem1, 1);
        Inventory.Contents[2] = (Items.JakeVoodooDoll, 1);
    }

    public override void Update()
    {
        base.Update();

        if (Health <= 0)
            return;
        
        // Check if we're in water
        bool inWater = World.BaseLayer.TileAtWorldCoord(Center) == Tiles.Tiles.Water;
        if (inWater)
        {
            SetAnimation("in_water");
            SpeedMultiplier = 0.3f;
        }
        else SpeedMultiplier = 1;

        if (World.BaseLayer.TileAtWorldCoord(Center) == Tiles.Tiles.DeepWater)
        {
            Drown();
            return;
        }
        
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

        if (Raylib.IsKeyDown(KeyboardKey.One))
            SelectedHotbarSlot = 0;
        if (Raylib.IsKeyDown(KeyboardKey.Two))
            SelectedHotbarSlot = 1;
        if (Raylib.IsKeyDown(KeyboardKey.Three))
            SelectedHotbarSlot = 2;
        if (Raylib.IsKeyDown(KeyboardKey.Four))
            SelectedHotbarSlot = 3;
        if (Raylib.IsKeyDown(KeyboardKey.Five))
            SelectedHotbarSlot = 4;
        if (Raylib.IsKeyDown(KeyboardKey.Six))
            SelectedHotbarSlot = 5;

        SelectedHotbarSlot += (int)Raylib.GetMouseWheelMove();
        SelectedHotbarSlot = Math.Clamp(SelectedHotbarSlot, 0, Inventory.RowSize - 1);
        
        if (Raylib.IsKeyPressed(KeyboardKey.H))
            Hurt(1);

        if (Raylib.IsKeyPressed(KeyboardKey.B))
        {
            Bomb bomb = new();
            bomb.Position = Position;
            var mousePos = Game.Instance.ScreenPosToWorld(Raylib.GetMousePosition());
            bomb.Velocity = Vector2.Normalize(mousePos - Position) * 100;
            World.AddEntity(bomb);
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            (Item.Item? heldItem, _) = Inventory.Contents[SelectedHotbarSlot];
            heldItem?.OnUsed();
            
            Vector2 worldPos = Game.Instance.ScreenPosToWorld(Raylib.GetMousePosition());
            (int x, int y) = World.TopLayer.WorldCoordToTileCoord(worldPos);
            if (x > 0 && x < World.TopLayer.Width && y > 0 && y < World.TopLayer.Height)
            {
                Tile? tile = World.TopLayer.TileAtTileCoord(x, y);

                if (tile is not null)
                {
                    World.TopLayer.DestroyTile(x, y);
                }
            }
        }
        
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

            if (!inWater && Health > 0) SetAnimation("walk");
        }
        else
        {
            if (!inWater && Health > 0) SetAnimation("idle");
        }
        
        Vector2 lerpedMovement = Utility.LerpSmooth(_lastLerpedMovement, _lastMovement, 0.02f);
        _lastLerpedMovement = lerpedMovement;
        Rot = (float)Math.Atan2(lerpedMovement.Y, lerpedMovement.X) * (180 / MathF.PI);
        
        Game.Instance.CameraTarget = Center;
    }

    public void Drown()
    {
        _drowned = true;
        Hurt(Health);
    }

    public override void Die()
    {
        SetAnimation(_drowned ? "drowned" : "dead");
        
        if (_drowned)
            Game.Instance.AttachLayer(new MenuLayer(new DeadMenu(this, "YOU DROWNED", "Deep water will kill you")), Game.Instance.GetLayerCount() - 1);
        else
            Game.Instance.AttachLayer(new MenuLayer(new DeadMenu(this)), Game.Instance.GetLayerCount() - 1);
    }

    public void Respawn()
    {
        Center = World.SpawnPoint;
        Health = 10;
        _drowned = false;
        ImmunityTimer = 3f;
    }
}
