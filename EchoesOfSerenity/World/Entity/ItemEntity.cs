using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.World.Entity;

public class ItemEntity : Core.Entity.Entity
{
    public Item.Item Item;
    public int Count;
    public static float PickupRangeSquared = 35 * 35;
    public static Font? tooltipFont = null;
    
    private bool _pickedUp = false;
    private Vector2 _velocity;

    public ItemEntity(Item.Item item, int count = 1)
    {
        Item = item;
        Count = count;

        Size = new Vector2(16, 16);
        
        if (tooltipFont is null)
            tooltipFont = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 18);

        Random rnd = new();
        _velocity = new Vector2(rnd.Next(-48, 48), rnd.Next(-48, 48));
    }

    public override void Update()
    {
        base.Update();

        if (_pickedUp)
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
        
        Position += _velocity * Raylib.GetFrameTime();
        _velocity -= _velocity * Raylib.GetFrameTime() * 5;
        
        if (Raymath.Vector2DistanceSqr(World.Player.Center, Center) < PickupRangeSquared)
        {
            _pickedUp = true;
        }
    }

    public override void Render()
    {
        base.Render();
        
        if (!Raylib.CheckCollisionRecs(Game.Instance.CameraBounds, BoundingBox))
            return;
        
        Raylib.DrawTexturePro(Item.Texture, new Rectangle(0, 0, Item.Texture.Width, Item.Texture.Height),
            BoundingBox, new Vector2(0, 0),
            0, Color.White);

        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), BoundingBox))
        {
            Raylib.DrawTextEx(tooltipFont!.Value, $"{Item.Name}{(Count > 1 ? $" ({Count})" : "")}", new Vector2(Raylib.GetMouseX() + 16, Raylib.GetMouseY() + 16), 18, 1, Color.White);
        }
    }
}
