using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;

namespace EchoesOfSerenity.World.Entity;

public static class Spritesheets
{
    public static Spritesheet Player = new(30, 30);
    public static Spritesheet Bomb = new(16, 16);
    
    public static void Init()
    {
        Player.SetTexture(ContentManager.GetTexture("Content/Spritesheets/Player.png"));
        Player.AddAnimation("idle", 0, 0, 1, 1);
        Player.AddAnimation("walk", 1, 0, 2, 3);
        Player.AddAnimation("in_water", 0, 1, 4, 4);
        Player.AddAnimation("drowned", 0, 2, 1, 1);
        Player.AddAnimation("dead", 1, 2, 1, 1);
        
        Bomb.SetTexture(ContentManager.GetTexture("Content/Spritesheets/Bomb.png"));
        Bomb.AddAnimation("blow", 0, 0, 5, 2.5f);
    }
}
