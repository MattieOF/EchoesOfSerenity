using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.Core.Entity;
using EchoesOfSerenity.World.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.World.Item;

public class BombItem : Item
{
    public BombItem()
    {
        Name = "Bomb";
        Description = "Will explode after a short delay when thrown.";
        Texture = ContentManager.GetTexture("Content/Items/Bomb.png");
        Consumable = true;
        UseType = UseType.Consumable;
    }
    
    public override bool OnUsed(LivingEntity user)
    {
        BombEntity bomb = new()
        {
            Position = user.Center
        };
        var mousePos = Game.Instance.ScreenPosToWorld(Raylib.GetMousePosition());
        bomb.Velocity = Vector2.Normalize(mousePos - user.Center) * 100;
        user.World.AddEntity(bomb);
        return true;
    }
}
