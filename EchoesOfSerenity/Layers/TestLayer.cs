using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.Layers;

public class TestLayer : ILayer
{
    public void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.J))
            SoundManager.PlaySound(ContentManager.GetSound("Content/Sounds/jerma.wav"));
    }
}
