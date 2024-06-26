using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.UI.Menus;

public class PauseMenu : Menu
{
    public PauseMenu()
    {
        Background = new Color(0, 0, 0, 160);
        
        Label title = new();
        title.Text = "Game Paused";
        title.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 40);
        title.FontSize = 40;
        title.Position = new Vector2(80, 50);
        title.HorizontalAlignment = HorizontalAlignment.Left;
        title.HorizontalAnchor = HorizontalAlignment.Left;
        AddElement(title);
        
        Button resume = new();
        resume.OnPressed.Add(_ =>
        {
            Game.Instance.IsPaused = false;
            RemoveFromParent();
            return true;
        });
        resume.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 30);
        resume.FontSize = 30;
        resume.Padding = new Vector2(20, 10);
        resume.Text = "Resume";
        resume.Position = new Vector2(80, 150);
        resume.HorizontalAlignment = HorizontalAlignment.Left;
        resume.VerticalAlignment = VerticalAlignment.Bottom;
        resume.HorizontalAnchor = HorizontalAlignment.Left;
        resume.VerticalAnchor = VerticalAlignment.Bottom;
        AddElement(resume);

        Button quit = new();
        quit.OnPressed.Add(_ =>
        {
            Game.Instance.IsPaused = false;
            Echoes.EchoesInstance.AttachLayer(new MenuLayer(new LoadingMenu(LoadingMenu.LoadTo.Menu)), Echoes.EchoesInstance.GetLayerCount() - 1);
            RemoveFromParent();
            return true;
        });
        quit.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 30);
        quit.FontSize = 30;
        quit.Padding = new Vector2(20, 10);
        quit.Text = "Return to Menu";
        quit.Position = new Vector2(80, 100);
        quit.HorizontalAlignment = HorizontalAlignment.Left;
        quit.VerticalAlignment = VerticalAlignment.Bottom;
        quit.HorizontalAnchor = HorizontalAlignment.Left;
        quit.VerticalAnchor = VerticalAlignment.Bottom;
        AddElement(quit);
    }

    public override void Update()
    {
        base.Update();
        
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            RemoveFromParent();
            Game.Instance.IsPaused = false;
        }
    }
}
