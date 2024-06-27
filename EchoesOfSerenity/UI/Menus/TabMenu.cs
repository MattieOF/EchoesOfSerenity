using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.UI.Menus;

public class TabMenu : Menu
{
    public Font TabNameFont;
    public int TabNameSize = 30;
    public Color TabNameColor = Color.White, SelectedTabNameColor = Color.Black;
    public Color TabBGColor = Color.Black, SelectedTabBGColor = Color.White;
    public int TabUIY = 50, TabUIHeight = 50;
    public List<(string name, Menu menu)> Tabs = [];
    public int ActiveTab = 0;

    private float _currentHighlightX = 0;

    public TabMenu()
    {
        TabNameFont = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", TabNameSize);
    }

    public override void Layout()
    {
        base.Layout();
        
        foreach (var (_, menu) in Tabs)
            menu.Layout();
    }

    public override void Update()
    {
        base.Update();

        int prevTab = ActiveTab;
        
        if (Raylib.IsKeyPressed(KeyboardKey.Q))
            ActiveTab = Math.Clamp(ActiveTab - 1, 0, Tabs.Count - 1);
        else if (Raylib.IsKeyPressed(KeyboardKey.E))
            ActiveTab = Math.Clamp(ActiveTab + 1, 0, Tabs.Count - 1);

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            var mousePos = Raylib.GetMousePosition();
            int screenWidth = Raylib.GetScreenWidth();
            int tabSize = screenWidth / Tabs.Count;
            
            if (mousePos.Y >= TabUIY && mousePos.Y <= TabUIY + TabUIHeight)
            {
                ActiveTab = (int)(mousePos.X / tabSize);
            }
        }
        
        if (ActiveTab != prevTab)
            Tabs[prevTab].menu.OnClosed();
        
        Tabs[ActiveTab].menu.Update();
    }

    public override void Render()
    {
        base.Render();
        
        // Render tab menu
        int screenWidth = Raylib.GetScreenWidth();
        int tabSize = screenWidth / Tabs.Count;
        int cx = 0;

        _currentHighlightX = Utility.LerpSmooth(_currentHighlightX, ActiveTab * tabSize, 0.005f);
        
        Raylib.DrawRectangle(0, TabUIY, screenWidth, TabUIHeight, TabBGColor);
        Raylib.DrawRectangle((int)_currentHighlightX, TabUIY, tabSize, TabUIHeight, SelectedTabBGColor);
        
        for (int i = 0; i < Tabs.Count; i++)
        {
            (string name, _) = Tabs[i];
            var size = Raylib.MeasureTextEx(TabNameFont, name, TabNameSize, 0);
            Vector2 textPos = new Vector2(cx + (tabSize - size.X) / 2, TabUIY + (TabUIHeight - size.Y) / 2);
            Raylib.DrawTextEx(TabNameFont, name, textPos, TabNameSize, 0, 
                ((textPos.X > _currentHighlightX && textPos.X < _currentHighlightX + tabSize) || (textPos.X < _currentHighlightX && textPos.X > _currentHighlightX + tabSize)) ? SelectedTabNameColor : TabNameColor);
            cx += tabSize;
        }
        
        Tabs[ActiveTab].menu.Render();
    }
}
