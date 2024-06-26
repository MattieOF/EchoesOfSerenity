using System.Numerics;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.UI.Menus;

public class LoadingMenu : Menu
{
    public enum LoadTo
    {
        Game,
        Menu
    }
    
    public float LoadTimer = 0.05f;
    public float FadeOut = 1.2f;

    private Label _title, _tip;
    private bool _inGame = false;
    private LoadTo _loadTo;
    
    public static List<string> Tips =
    [
        "Just use an engine, for your own sake"
    ];
    
    public LoadingMenu(LoadTo loadTo)
    {
        _loadTo = loadTo;
        Background = Color.Black;
        
        _title = new();
        _title.Text = "Loading";
        _title.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 40);
        _title.FontSize = 40;
        _title.Position = new Vector2(0, 0);
        _title.HorizontalAlignment = HorizontalAlignment.Center;
        _title.VerticalAlignment = VerticalAlignment.Middle;
        _title.HorizontalAnchor = HorizontalAlignment.Center;
        _title.VerticalAnchor = VerticalAlignment.Middle;
        AddElement(_title);
        
        _tip = new();
        _tip.Text = Tips[new Random().Next(0, Tips.Count)];
        _tip.Colour = Color.Gray;
        _tip.Font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 25);
        _tip.FontSize = 25;
        _tip.Position = new Vector2(0, 50);
        _tip.HorizontalAlignment = HorizontalAlignment.Center;
        _tip.VerticalAlignment = VerticalAlignment.Bottom;
        _tip.HorizontalAnchor = HorizontalAlignment.Center;
        _tip.VerticalAnchor = VerticalAlignment.Bottom;
        AddElement(_tip);
    }

    public override void Update()
    {
        base.Update();

        if (LoadTimer > 0)
        {
            LoadTimer -= Raylib.GetFrameTime();
            if (LoadTimer <= 0)
            {
                switch (_loadTo)
                {
                    case LoadTo.Menu:
                        Echoes.EchoesInstance.ReturnToMenu();
                        break;
                    case LoadTo.Game:
                        Echoes.EchoesInstance.StartGame();
                        break;
                }
            }
        }
        else
        {
            if (!_inGame)
            {
                _inGame = true;
                return;
            }

            FadeOut -= Raylib.GetFrameTime();
            _tip.Colour.A = (byte)Raymath.Lerp(0, 255, Math.Clamp(FadeOut, 0, 1));
            _title.Colour.A = (byte)Raymath.Lerp(0, 255, Math.Clamp(FadeOut, 0, 1));
            Background.A = (byte)Raymath.Lerp(0, 255, Math.Clamp(FadeOut, 0, 1));

            if (FadeOut <= 0)
            {
                RemoveFromParent();
            }
        }
    }
}
