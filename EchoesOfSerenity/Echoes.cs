using EchoesOfSerenity.Core;
using EchoesOfSerenity.Layers;
using EchoesOfSerenity.UI;
using EchoesOfSerenity.UI.Menus;
using EchoesOfSerenity.World;
using EchoesOfSerenity.World.Gen;
using EchoesOfSerenity.World.Item;
using EchoesOfSerenity.World.Tiles;
using Raylib_cs;

namespace EchoesOfSerenity;

public class Echoes : Game
{
    public static Echoes EchoesInstance = null!;
    public World.World World = null!;
    public HUDLayer? HUD = null;
    public WorldLayer? WorldLayer = null;
    
    protected override void OnInit()
    {
        EchoesInstance = this;
        
        Tiles.Init();
        Items.PostTileInit();
        Recipes.Init();

        ConstructLayer<DebugLayer>();
        
        AttachLayer(new MenuLayer(new MainMenu()));
    }

    public void StartGame()
    {
        IsPaused = false;
        if (HUD is not null)
            DetachLayer(HUD);
        HUD = ConstructLayer<HUDLayer>();
        World = WorldGen.GenerateWorld(32, 32);
        WorldLayer = new(World);
        AttachLayer(WorldLayer);
    }

    protected override void OnUpdate()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.F11))
            Raylib.ToggleBorderlessWindowed();
    }

    public void ReturnToMenu()
    {
        if (HUD is not null)
            DetachLayer(HUD);
        if (WorldLayer is not null)
            DetachLayer(WorldLayer);
        World.Dispose();
        
        AttachLayer(new MenuLayer(new MainMenu()));
    }
}
