using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.Core.Tilemap;

public class Tileset
{
    public Texture2D TilesetTexture { get; private set; }
    public int TileWidth, TileHeight;
    
    public Tileset(string filepath, int width, int height)
    {
        TilesetTexture = ContentManager.GetTexture(filepath);
        TileWidth = width;
        TileHeight = height;

        if (TilesetTexture.Width % TileWidth != 0
            || TilesetTexture.Height % TileHeight != 0)
        {
            Utility.WriteLineColour(ConsoleColor.Red, $"Tileset image size does not match tile size.");
        }
    }
}
