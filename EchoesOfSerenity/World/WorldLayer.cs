using EchoesOfSerenity.Core;

namespace EchoesOfSerenity.World;

public class WorldLayer(World world) : ILayer
{
    public World World { get; private set; } = world;

    public void PreRender()
    {
        World.PreRender();
    }

    public void Render()
    {
        World.Render();
    }
}
