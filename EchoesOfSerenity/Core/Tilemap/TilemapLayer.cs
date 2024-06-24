using EchoesOfSerenity.World;
using Raylib_cs;

namespace EchoesOfSerenity.Core.Tilemap;

public class TilemapLayer(Tilemap tilemap) : ILayer
{
    public Tilemap Tilemap = tilemap;

    public void Update()
    {
        if (Raylib.IsKeyDown(KeyboardKey.F))
        {
            Random rnd = new();
            int x = rnd.Next(0, Tilemap.Width);
            int y = rnd.Next(0, Tilemap.Height);
            Tilemap.SetTile(x, y, rnd.Next(0, 2) == 0 ? Tiles.Grass : Tiles.Water);
        }
    }

    public void PreRender()
    {
        Tilemap.PreRender();
    }

    public void Render()
    {
        Tilemap.Render();
    }
}
