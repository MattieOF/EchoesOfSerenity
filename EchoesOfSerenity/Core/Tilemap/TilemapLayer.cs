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
            // Tilemap.SetTile(x, y, rnd.Next(0, 5) == 0 ? Tiles.Water : Tiles.Grass);
            Tilemap.SetTile(x, y, Tiles.Grass);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.G))
        {
            Random rnd = new();
            for (int x = 0; x < Tilemap.Width; x++)
            {
                for (int y = 0; y < Tilemap.Height; y++)
                {
                    Tilemap.SetTile(x, y, rnd.Next(0, 5) == 0 ? Tiles.Water : Tiles.Grass);
                }
            }
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
