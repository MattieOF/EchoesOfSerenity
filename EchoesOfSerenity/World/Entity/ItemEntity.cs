using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.UI;
using Raylib_cs;

namespace EchoesOfSerenity.World.Entity;

public class ItemEntity : Core.Entity.Entity
{
    public Item.Item Item;
    public int Count;
    public static float PickupRangeSquared = 35 * 35;
    private static Font? _tooltipFont = null;
    private static Texture2D? _frame = null;
    public Vector2 Velocity;
    
    private bool _pickedUp = false, _visible = false;

    public ItemEntity(Item.Item item, int count = 1)
    {
        Item = item;
        Count = count;

        Size = new Vector2(16, 16);
        
        if (_tooltipFont is null)
            _tooltipFont = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 18);
        if (_frame is null)
            _frame = ContentManager.GetTexture("Content/UI/Frame.png");
        
        Random rnd = new();
        Velocity = new Vector2(rnd.Next(-48, 48), rnd.Next(-48, 48));
    }

    public override void Update()
    {
        base.Update();

        if (_pickedUp && World.Player.Health > 0)
        {
            Center += Raymath.Vector2Normalize(World.Player.Center - Center) * Raylib.GetFrameTime() * 128;
            if (Raymath.Vector2DistanceSqr(World.Player.Center, Center) < 4)
            {
                var leftover = World.Player.Inventory.AddItem(Item, Count);
                if (leftover == 0)
                    Kill();
                else
                {
                    Count = leftover;
                    _pickedUp = false;
                }
            }
            return;
        }
        
        Position += Velocity * Raylib.GetFrameTime();
        Velocity -= Velocity * Raylib.GetFrameTime() * 5;
        
        if (World.Player.Health > 0 && Raymath.Vector2DistanceSqr(World.Player.Center, Center) < PickupRangeSquared)
        {
            _pickedUp = true;
        }
    }

    public override void Render()
    {
        base.Render();

        _visible = Raylib.CheckCollisionRecs(Game.Instance.CameraBounds, BoundingBox);
        if (!_visible)
            return;
        
        Raylib.DrawTexturePro(Item.Texture, new Rectangle(0, 0, Item.Texture.Width, Item.Texture.Height),
            BoundingBox, new Vector2(0, 0),
            0, Color.White);
    }

    public override void RenderUI()
    {
        base.RenderUI();

        if (!_visible)
            return;
        
        var mousePos = Raylib.GetMousePosition();
        if (Raylib.CheckCollisionPointRec(Game.Instance.ScreenPosToWorld(mousePos), BoundingBox))
        {
            var tooltip = $"{Item.Name}{(Count > 1 ? $" ({Count})" : "")}";
            var size = Raylib.MeasureTextEx(_tooltipFont!.Value, tooltip, 18, 0);
            var origin = new Vector2(mousePos.X + 16, mousePos.Y + 16);
            Raylib.DrawTextureNPatch(_frame!.Value, Menu.FrameNPatch, new Rectangle(origin, size + new Vector2(24, 10)), Vector2.Zero, 0, Color.White);
            Raylib.DrawTextEx(_tooltipFont!.Value, tooltip, origin + new Vector2(12, 5), 18, 0, Color.White);
        }
    }
}
