using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.World.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.UI;

public class HUDLayer : ILayer
{
    public PlayerEntity Player = null!;
    public Texture2D HeartTexture;
    
    public void OnAttach()
    {
        HeartTexture = ContentManager.GetTexture("Content/UI/Heart.png");
    }

    public void RenderUI()
    {
        if (Player is null) 
            return;
        
        for (int i = 0; i < 5; i++)
        {
            Raylib.DrawTexturePro(HeartTexture, new Rectangle(0, 0, HeartTexture.Width, HeartTexture.Height), new Rectangle(10 + i * 34, 10, 32, 32), new Vector2(0, 0), 0, Color.Black);    
        }
    }
}
