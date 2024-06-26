using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.World.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.UI;

public class HUDLayer : ILayer
{
    public PlayerEntity Player = null!;
    public Texture2D HeartTexture, FlashingHeartTexture;
    
    public void OnAttach()
    {
        HeartTexture = ContentManager.GetTexture("Content/UI/Heart.png");
        FlashingHeartTexture = ContentManager.GetTexture("Content/UI/HeartFlash.png");
    }

    public void RenderUI()
    {
        if (Player is null) 
            return;

        int hp = (int) Player.Health;
        
        for (int i = 0; i < 5; i++)
        {
            Raylib.DrawTexturePro(HeartTexture, new Rectangle(0, 0, HeartTexture.Width, HeartTexture.Height), new Rectangle(10 + i * 34, 10, 32, 32), new Vector2(0, 0), 0, Color.Black);
            if (hp > 0)
            {
                Raylib.DrawTexturePro(Player.ImmunityTimer > 0 ? FlashingHeartTexture : HeartTexture, new Rectangle(0, 0, hp == 1 ? HeartTexture.Width / 2.0f : HeartTexture.Width, HeartTexture.Height),
                    new Rectangle(10 + i * 34, 10, hp == 1 ? 16 : 32, 32), new Vector2(0, 0), 0, Color.White);
                hp -= 2;
            }
        }
    }
}
