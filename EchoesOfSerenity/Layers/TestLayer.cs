using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.Layers;

public class TestLayer : Layer
{
    // public bool _demoWindow = false;
    //
    // public override void RenderImGUI()
    // {
    //     ImGui.ShowDemoWindow(ref _demoWindow);
    // }

    public override void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.J))
            SoundManager.PlaySound(ContentManager.GetSound("Content/Sounds/jerma.wav"));
    }
}
