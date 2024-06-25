using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.Layers;
using EchoesOfSerenity.World;
using Raylib_cs;

namespace EchoesOfSerenity;

public class Echoes : Game
{
    public static Tilemap Tilemap = null!;
    
    protected override void OnInit()
    {
        Tiles.Init();

        ConstructLayer<TestLayer>();
        ConstructLayer<DebugLayer>();

        Tileset tileset = new("Content/Spritesheets/TerrainSpritesheet.png", 16, 16);
        Tilemap = new(512, 512, tileset);
        TilemapLayer layer = new(Tilemap);
        AttachLayer(layer);
    }

    protected override void OnUpdate()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.F11))
            Raylib.ToggleBorderlessWindowed();
    }
}
