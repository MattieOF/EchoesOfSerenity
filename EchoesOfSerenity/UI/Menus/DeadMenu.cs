using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.World.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.UI.Menus;

public class DeadMenu : Menu
{
    private PlayerEntity _player;
    
    public DeadMenu(PlayerEntity player)
    {
        _player = player;
        Background = new Color(255, 0, 0, 100);
        
        Label title = new();
        title.Text = "YOU DIED";
        title.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 40);
        title.FontSize = 40;
        title.Position = new Vector2(0, 50);
        title.HorizontalAlignment = HorizontalAlignment.Center;
        title.HorizontalAnchor = HorizontalAlignment.Center;
        AddElement(title);
        
        Button repsawn = new();
        repsawn.OnPressed.Add(_ =>
        {
            _player.Respawn();
            RemoveFromParent();
            return true;
        });
        repsawn.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 30);
        repsawn.FontSize = 30;
        repsawn.Padding = new Vector2(20, 10);
        repsawn.Text = "Respawn";
        repsawn.Position = new Vector2(0, 150);
        repsawn.HorizontalAlignment = HorizontalAlignment.Center;
        repsawn.VerticalAlignment = VerticalAlignment.Bottom;
        repsawn.HorizontalAnchor = HorizontalAlignment.Center;
        repsawn.VerticalAnchor = VerticalAlignment.Bottom;
        AddElement(repsawn);

        Button quit = new();
        quit.OnPressed.Add(_ =>
        {
            Echoes.EchoesInstance.AttachLayer(new MenuLayer(new LoadingMenu(LoadingMenu.LoadTo.Menu)));
            RemoveFromParent();
            return true;
        });
        quit.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 30);
        quit.FontSize = 30;
        quit.Padding = new Vector2(20, 10);
        quit.Text = "Return to Menu";
        quit.Position = new Vector2(0, 100);
        quit.HorizontalAlignment = HorizontalAlignment.Center;
        quit.VerticalAlignment = VerticalAlignment.Bottom;
        quit.HorizontalAnchor = HorizontalAlignment.Center;
        quit.VerticalAnchor = VerticalAlignment.Bottom;
        AddElement(quit);
        
        Label _tip = new();
        _tip.Text = LoadingMenu.Tips[new Random().Next(0, LoadingMenu.Tips.Count)];
        _tip.Colour = new Color(200, 200, 200, 255);
        _tip.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 25);
        _tip.FontSize = 25;
        _tip.Position = new Vector2(0, 50);
        _tip.HorizontalAlignment = HorizontalAlignment.Center;
        _tip.VerticalAlignment = VerticalAlignment.Bottom;
        _tip.HorizontalAnchor = HorizontalAlignment.Center;
        _tip.VerticalAnchor = VerticalAlignment.Bottom;
        AddElement(_tip);
    }
}
