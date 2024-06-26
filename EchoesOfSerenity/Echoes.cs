using EchoesOfSerenity.Core;
using EchoesOfSerenity.Layers;
using EchoesOfSerenity.UI;
using EchoesOfSerenity.World;
using EchoesOfSerenity.World.Gen;
using EchoesOfSerenity.World.Tiles;
using Raylib_cs;

namespace EchoesOfSerenity;

public class Echoes : Game
{
    public static Echoes EchoesInstance = null!;
    public World.World World = null!;
    public HUDLayer HUD = null!;
    
    protected override void OnInit()
    {
        EchoesInstance = this;
        
        Tiles.Init();

        ConstructLayer<TestLayer>();
        ConstructLayer<DebugLayer>();
        HUD = ConstructLayer<HUDLayer>();

        // TODO: async load
        World = WorldGen.GenerateWorld(32, 32);
        WorldLayer worldLayer = new(World);
        AttachLayer(worldLayer);
    }

    protected override void OnUpdate()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.F11))
            Raylib.ToggleBorderlessWindowed();
    }
}
