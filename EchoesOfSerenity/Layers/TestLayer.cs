using EchoesOfSerenity.Core;
using ImGuiNET;

namespace EchoesOfSerenity.Layers;

public class TestLayer : Layer
{
    public bool _demoWindow = false;
    
    public override void RenderImGUI()
    {
        ImGui.ShowDemoWindow(ref _demoWindow);
    }
}
