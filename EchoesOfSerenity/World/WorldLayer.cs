using EchoesOfSerenity.Core;
using EchoesOfSerenity.UI;
using EchoesOfSerenity.UI.Menus;
using Raylib_cs;

namespace EchoesOfSerenity.World;

public class WorldLayer(World world) : ILayer
{
    public World World { get; private set; } = world;

    public void Update()
    {
        if (!Game.Instance.IsPaused)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                Game.Instance.IsPaused = true;
                Game.Instance.AttachLayer(new MenuLayer(new PauseMenu()), Game.Instance.GetLayerCount() - 1);
                return;
            }
            
            World.Update();
        }
    }

    public void PreRender()
    {
        World.PreRender();
    }

    public void Render()
    {
        World.Render();
    }
}
