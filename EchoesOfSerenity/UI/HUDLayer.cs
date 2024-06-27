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
    private Font _itemNameFont, _itemDescFont;

    public static int SlotSize = 50;
    
    public void OnAttach()
    {
        _heartTexture = ContentManager.GetTexture("Content/UI/Heart.png");
        _flashingHeartTexture = ContentManager.GetTexture("Content/UI/HeartFlash.png");
        _itemNameFont = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 20);
        _itemDescFont = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 12);
    }

    public void RenderUI()
    {
        if (Player is null) 
            return;

        int hp = Player.Health == 0 ? 0 : Math.Clamp((int)Player.Health, 1, int.MaxValue);
                    
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

        (Item? selectedItem, _) = Player.Inventory.Contents[Player.SelectedHotbarSlot];
        if (selectedItem is not null)
        {
            bool hasDesc = !string.IsNullOrWhiteSpace(selectedItem.Description);
            var size = Raylib.MeasureTextEx(_itemNameFont, selectedItem.Name, 20, 0);
            Raylib.DrawTextEx(_itemNameFont, selectedItem.Name, new Vector2(Raylib.GetScreenWidth() / 2.0f - size.X / 2, hasDesc ? 10 : 20), 20, 0, Color.White);
            if (hasDesc)
            {
                size = Raylib.MeasureTextEx(_itemDescFont, selectedItem.Description, 12, 0);
                Raylib.DrawTextEx(_itemDescFont, selectedItem.Description, new Vector2(Raylib.GetScreenWidth() / 2.0f - size.X / 2, 30), 12, 0, Color.White);
            }
        }
        
        int invBarX = ((int)(Raylib.GetScreenWidth()) / 2) - SlotSize * (Inventory.RowSize / 2);
        for (int i = 0; i < Inventory.RowSize; i++)
        {
            (Item? item, int count) = Player.Inventory.Contents[i];
            Raylib.DrawRectangle(invBarX, 50, SlotSize, SlotSize, Player.SelectedHotbarSlot == i ? Color.Gray : Color.DarkGray);
            if (item is not null)
            {
                Raylib.DrawTexturePro(item.Texture, new Rectangle(0, 0, item.Texture.Width, item.Texture.Height),
                    new Rectangle(invBarX + 5, 50 + 5, SlotSize - 10, SlotSize - 10), new Vector2(0, 0), 0, Color.White);
                
                if (count is not 1)
                {
                    Raylib.DrawTextEx(_itemNameFont, count.ToString(), new Vector2(invBarX + 5, 50 + 2), 20, 0, Color.White);
                }
            }
            
            Raylib.DrawRectangleLinesEx(new Rectangle(invBarX, 50, SlotSize, SlotSize), 2, Color.Black);
            invBarX += SlotSize;
        }
    }
}
