using Raylib_cs;

namespace EchoesOfSerenity.Core.Tilemap;

public class Tilemap : IDisposable
{
    public Tileset Tileset;

    private int _width, _height;
    private Tile[,] _tiles;
    private const int ChunkSize = 16;
    private List<RenderTexture2D> _chunks = new();

    public Tilemap(int width, int height, Tileset tileset)
    {
        if (width % ChunkSize != 0 || height % ChunkSize != 0)
        {
            Utility.WriteLineColour(ConsoleColor.Red, $"Tilemap size must be divisible by the chunk size ({ChunkSize}");
        }
        
        Tileset = tileset;
        _width = width;
        _height = height;
        _tiles = new Tile[width, height];
    }

    public void SetTile(int x, int y, Tile tile)
    {
        
    }

    public void Render()
    {
        
    }

    public void RenderChunk(int index)
    {
        
    }

    public void Dispose()
    {
        foreach (var texture in _chunks)
            Raylib.UnloadRenderTexture(texture);
    }
}
