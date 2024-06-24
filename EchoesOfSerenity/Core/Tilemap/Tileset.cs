using System.Numerics;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.Core.Tilemap;

public class Tileset
{
    public Texture2D TilesetTexture { get; private set; }
    public int TileWidth, TileHeight;
    public int TileCount => (TilesetTexture.Width / TileWidth) * (TilesetTexture.Height / TileHeight);
    public int TileColumns => TilesetTexture.Width / TileWidth;
    public int TileRows => TilesetTexture.Height / TileHeight;
    
    public Tileset(string filepath, int tileWidth, int tileHeight)
    {
        TilesetTexture = ContentManager.GetTexture(filepath);
        TileWidth = tileWidth;
        TileHeight = tileHeight;

        if (TilesetTexture.Width % TileWidth != 0
            || TilesetTexture.Height % TileHeight != 0)
        {
            Utility.WriteLineColour(ConsoleColor.Red, $"Tileset image size does not match tile size.");
        }
    }
    
    public void RenderTile(int x, int y, int tileX, int tileY)
    {
        Raylib.DrawTextureRec(TilesetTexture, new Rectangle(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight), new Vector2(x, y), Color.White);
    }
}
