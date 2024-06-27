using System.Numerics;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.World.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.UI.Menus;

public class StatMenu : Menu
{
    private readonly PlayerEntity _player;
    private Font _font, _headerFont;
    
    public StatMenu(PlayerEntity player)
    {
        _player = player;
        _font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 18);
        _headerFont = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 40);
    }
    
    public override void Render()
    {
        int x = 50, y = 150;
        Raylib.DrawTextEx(_headerFont, "Stats", new Vector2(x, y), 40, 0, Color.White);
        y += 50;
        foreach (var stat in _player.Stats.StatList)
        {
            Raylib.DrawTextEx(_font, $"{stat.Value.Name}: {stat.Value.Value:N0} {stat.Value.Unit}", new Vector2(x, y), 18, 0, Color.White);
            y += 20;
        }
    }
}
