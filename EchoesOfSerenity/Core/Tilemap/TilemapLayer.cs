using Raylib_cs;

namespace EchoesOfSerenity.Core.Tilemap;

public class TilemapLayer(Tilemap tilemap) : Layer
{
    public Tilemap Tilemap = tilemap;

    public override void Render()
    {
        Raylib.DrawTexture(Tilemap.Tileset.TilesetTexture, 10, 10, Color.White);
    }
}
