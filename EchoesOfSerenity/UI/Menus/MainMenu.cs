using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.UI.Menus;

public class MainMenu : Menu
{
    public MainMenu() 
        : base()
    {
        Background = Color.Black;
        
        Label title = new();
        title.Text = "Echoes of Serenity";
        title.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 40);
        title.FontSize = 40;
        title.Position = new Vector2(0, 50);
        title.HorizontalAlignment = HorizontalAlignment.Center;
        title.HorizontalAnchor = HorizontalAlignment.Center;
        AddElement(title);

        Button newWorld = new();
        newWorld.OnPressed.Add(_ =>
        {
            Echoes.EchoesInstance.AttachLayer(new MenuLayer(new LoadingMenu()));
            RemoveFromParent();
            return true;
        });
        newWorld.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 30);
        newWorld.FontSize = 30;
        newWorld.Padding = new Vector2(20, 10);
        newWorld.Text = "New World";
        newWorld.Position = new Vector2(0, 100);
        newWorld.HorizontalAlignment = HorizontalAlignment.Center;
        newWorld.VerticalAlignment = VerticalAlignment.Middle;
        newWorld.HorizontalAnchor = HorizontalAlignment.Center;
        newWorld.VerticalAnchor = VerticalAlignment.Middle;
        AddElement(newWorld);
        
        Button loadWorld = new();
        loadWorld.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 30);
        loadWorld.FontSize = 30;
        loadWorld.Padding = new Vector2(20, 10);
        loadWorld.Text = "Load World";
        loadWorld.Position = new Vector2(0, 50);
        loadWorld.HorizontalAlignment = HorizontalAlignment.Center;
        loadWorld.VerticalAlignment = VerticalAlignment.Middle;
        loadWorld.HorizontalAnchor = HorizontalAlignment.Center;
        loadWorld.VerticalAnchor = VerticalAlignment.Middle;
        loadWorld.Tooltip = "Not implemented :(";
        AddElement(loadWorld);
        
        Button quit = new();
        quit.OnPressed.Add(_ =>
        {
            Game.Instance.CloseGame();
            return false;
        });
        quit.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 30);
        quit.FontSize = 30;
        quit.Padding = new Vector2(20, 10);
        quit.Text = "Quit";
        quit.Position = new Vector2(0, 0);
        quit.HorizontalAlignment = HorizontalAlignment.Center;
        quit.VerticalAlignment = VerticalAlignment.Middle;
        quit.HorizontalAnchor = HorizontalAlignment.Center;
        quit.VerticalAnchor = VerticalAlignment.Middle;
        AddElement(quit);

        ImageButton ugjLogo = new();
        ugjLogo.OnPressed.Add(_ =>
        {
            Raylib.OpenURL("https://sites.google.com/port.ac.uk/universitygamejam/home");
            return true;
        });
        ugjLogo.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 30);
        ugjLogo.Texture = ContentManager.GetTexture("Content/UI/UGJLogo.png");
        ugjLogo.Rect.Size = new Vector2(120, 120);
        ugjLogo.Tooltip = "An entry to the University Game Jam";
        ugjLogo.Position = new Vector2(20, 20);
        ugjLogo.HorizontalAlignment = HorizontalAlignment.Left;
        ugjLogo.VerticalAlignment = VerticalAlignment.Bottom;
        ugjLogo.HorizontalAnchor = HorizontalAlignment.Left;
        ugjLogo.VerticalAnchor = VerticalAlignment.Bottom;
        AddElement(ugjLogo);
        
        ImageButton raylibLogo = new();
        raylibLogo.OnPressed.Add(_ =>
        {
            Raylib.OpenURL("https://www.raylib.com/");
            return true;
        });
        raylibLogo.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 30);
        raylibLogo.Texture = ContentManager.GetTexture("Content/UI/RaylibLogo.png");
        raylibLogo.Rect.Size = new Vector2(120, 120);
        raylibLogo.Tooltip = "Created with Raylib";
        raylibLogo.Position = new Vector2(150, 20);
        raylibLogo.HorizontalAlignment = HorizontalAlignment.Left;
        raylibLogo.VerticalAlignment = VerticalAlignment.Bottom;
        raylibLogo.HorizontalAnchor = HorizontalAlignment.Left;
        raylibLogo.VerticalAnchor = VerticalAlignment.Bottom;
        AddElement(raylibLogo);
    }
}
