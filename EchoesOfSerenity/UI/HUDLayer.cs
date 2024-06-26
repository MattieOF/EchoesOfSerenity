using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.World.Entity;
using EchoesOfSerenity.World.Item;
using Raylib_cs;

namespace EchoesOfSerenity.UI;

public class HUDLayer : ILayer
{
    public PlayerEntity Player = null!;
    private Texture2D _heartTexture, _flashingHeartTexture;
    private Font _itemNameFont;

    public static int SlotSize = 50;
    
    public void OnAttach()
    {
        _heartTexture = ContentManager.GetTexture("Content/UI/Heart.png");
        _flashingHeartTexture = ContentManager.GetTexture("Content/UI/HeartFlash.png");
        _itemNameFont = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 20);
    }

    public void RenderUI()
    {
        if (Player is null) 
            return;

        int hp = (int) Player.Health;
        
        for (int i = 0; i < 5; i++)
        {
            Raylib.DrawTexturePro(_heartTexture, new Rectangle(0, 0, _heartTexture.Width, _heartTexture.Height), new Rectangle(10 + i * 34, 10, 32, 32), new Vector2(0, 0), 0, Color.Black);
            if (hp > 0)
            {
                Raylib.DrawTexturePro(Player.ImmunityTimer > 0 ? _flashingHeartTexture : _heartTexture, new Rectangle(0, 0, hp == 1 ? _heartTexture.Width / 2.0f : _heartTexture.Width, _heartTexture.Height),
                    new Rectangle(10 + i * 34, 10, hp == 1 ? 16 : 32, 32), new Vector2(0, 0), 0, Color.White);
                hp -= 2;
            }
        }

        (Item? item, _) = Player.Inventory.Contents[Player.SelectedHotbarSlot];
        if (item is not null)
        {
            var size = Raylib.MeasureTextEx(_itemNameFont, item.Name, 20, 0);
            Raylib.DrawTextEx(_itemNameFont, item.Name, new Vector2(Raylib.GetScreenWidth() / 2.0f - size.X / 2, 20), 20, 0, Color.Black);
        }
        
        int invBarX = ((int)(Raylib.GetScreenWidth()) / 2) - SlotSize * (Inventory.RowSize / 2);
        for (int i = 0; i < Inventory.RowSize; i++)
        {
            Raylib.DrawRectangle(invBarX, 50, SlotSize, SlotSize, Player.SelectedHotbarSlot == i ? Color.Gray : Color.DarkGray);
            Raylib.DrawRectangleLinesEx(new Rectangle(invBarX, 50, SlotSize, SlotSize), 2, Color.Black);
            invBarX += SlotSize;
        }
    }
}
