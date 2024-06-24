using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using ImGuiNET;
using Raylib_cs;

namespace EchoesOfSerenity.Layers;

public class DebugLayer : Layer
{
    private bool _fpsVisible = false;
    private const int FontSize = 18;
    private Font _font, _boldFont;
    private Vector2 _titleSize;
    private bool _debugMenuVisible = false;
    
    public override void OnAttach()
    {
        _font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf");
        _boldFont = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf");
        _titleSize = Raylib.MeasureTextEx(_boldFont, "EoS Stats", 18, 1);
    }

    public override void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.F1))
            _fpsVisible = !_fpsVisible;
        if (Raylib.IsKeyPressed(KeyboardKey.F2))
            _debugMenuVisible = !_debugMenuVisible;
    }

    public override void Render()
    {
        if (_fpsVisible)
        {
            // Proper UI system would make this way better, but this is a jam baybee
            string fpsString = $"FPS: {Raylib.GetFPS()}\nEntities: 0";
            Vector2 size = Raylib.MeasureTextEx(_font, fpsString, FontSize, 1);
            Raylib.DrawRectangle(15, 15, (int)(MathF.Max(size.X, _titleSize.X) + 20), (int)(size.Y + _titleSize.Y + 15), new Color(0, 0, 0, 130));
            Raylib.DrawTextEx(_boldFont, "EoS Stats", new Vector2(25, 20), FontSize, 1, Color.White);
            Raylib.DrawTextEx(_font, fpsString, new Vector2(25, 25 + _titleSize.Y), FontSize, 1, Color.White);
        }
    }

    public override void RenderImGUI()
    {
        if (_debugMenuVisible)
        {
            ImGui.Begin("EoS Debug Menu", ref _debugMenuVisible);
            if (ImGui.Button("Close")) 
                Game.Instance.CloseGame();
            ImGui.End();
        }
    }
}
