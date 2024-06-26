using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using Raylib_cs;

namespace EchoesOfSerenity.Core.Tilemap;

public class Tilemap : IDisposable
{
    public Tileset Tileset;

    public const int ChunkSize = 16;
    public int Width { get; private set; }
    public int Height { get; private set; }
    private Tile?[,] _tiles;
    public List<RenderTexture2D> Chunks { get; private set; } = new();
    private HashSet<int> _dirtyChunks = new();

    public static bool DrawChunkOutlines = false;
    public static bool EnableRandomRotation = true;
    public int RenderedChunks { get; private set; }

    public Tilemap(int width, int height, Tileset tileset)
    {
        if (width % ChunkSize != 0 || height % ChunkSize != 0)
        {
            Utility.WriteLineColour(ConsoleColor.Red, $"Tilemap size must be divisible by the chunk size ({ChunkSize}");
        }

        Tileset = tileset;
        Width = width;
        Height = height;
        _tiles = new Tile[width, height];
    }

    public void SetTile(int x, int y, Tile? tile)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            Utility.WriteLineColour(ConsoleColor.Red, $"Tile position ({x}, {y}) is out of bounds.");
            return;
        }

        _tiles[x, y] = tile;
        _dirtyChunks.Add((x / ChunkSize) + (y / ChunkSize) * (Width / ChunkSize));
    }

    public void PreRender()
    {
        if (_dirtyChunks.Count == 0) return;
        Stopwatch sw = new();
        sw.Start();
        
        foreach (var chunk in _dirtyChunks)
            RenderChunk(chunk);
        
        sw.Stop();
        Utility.WriteLineColour(ConsoleColor.Green, $"Rendered {_dirtyChunks.Count} chunks in {sw.Elapsed.TotalSeconds:F}s.");
        
        _dirtyChunks.Clear();
    }

    public void Render()
    {
        RenderedChunks = 0;
        
        // Render the chunks
        int x = 0, y = 0;
        Rectangle source = new Rectangle(0, 0, ChunkSize * Tileset.TileWidth, -ChunkSize * Tileset.TileHeight);
        for (var index = 0; index < Chunks.Count; index++)
        {
            // Make chunk bounding box
            Rectangle chunkRect = new(x, y, ChunkSize * Tileset.TileWidth, ChunkSize * Tileset.TileHeight);
            // And check if it's in the camera
            if (Raylib.CheckCollisionRecs(Game.Instance.CameraBounds, chunkRect))
            {
                Raylib.DrawTextureRec(Chunks[index].Texture, source, new Vector2(x, y), Color.White);
                
                // Draw animated tiles
                int chunkX = x / Tileset.TileWidth;
                int chunkY = y / Tileset.TileHeight;
                for (int cy = chunkY; cy < chunkY + ChunkSize; cy++) // Loop y first for cache efficiency
                {
                    for (int cx = chunkX; cx < chunkX + ChunkSize; cx++)
                    {
                        Tile? tile = _tiles[cx, cy];
                        if (tile is null || !tile.Animated) continue;
                        var (tilesetX, tilesetY) = Tileset.GetTileCoordinates(tile.TileSetIndex);
                        Tileset.RenderTile(cx * Tileset.TileWidth, cy * Tileset.TileHeight, tilesetX + (int)((Raylib.GetTime() * tile.FPS) % tile.Frames),
                            tilesetY);
                    }
                }
                
#if DEBUG
                if (DrawChunkOutlines)
                    Raylib.DrawRectangleLines(x, y, ChunkSize * Tileset.TileWidth, ChunkSize * Tileset.TileHeight,
                        Color.Red);
#endif

                RenderedChunks++;
            }

            x += ChunkSize * Tileset.TileWidth;
            if (x >= Width * Tileset.TileWidth)
            {
                x = 0;
                y += ChunkSize * Tileset.TileHeight;
            }
        }
    }

    public void RenderChunk(int index)
    {
        int chunkX = (index % (Width / ChunkSize)) * ChunkSize;
        int chunkY = (index / (Width / ChunkSize)) * ChunkSize;

        if (!Chunks.IsValidIndex(index))
        {
            if (Chunks.Count != index)
                Utility.WriteLineColour(ConsoleColor.Red, "Chunk index is out of order.");

            var tex = Raylib.LoadRenderTexture(ChunkSize * Tileset.TileWidth, ChunkSize * Tileset.TileHeight);
            Raylib.SetTextureWrap(tex.Texture, TextureWrap.Clamp);
            Chunks.Add(tex);
        }

        Raylib.BeginTextureMode(Chunks[index]);
        Raylib.ClearBackground(Color.Blank);

        for (int y = chunkY; y < chunkY + ChunkSize; y++) // Loop y first for cache efficiency
        {
            for (int x = chunkX; x < chunkX + ChunkSize; x++)
            {
                Tile? tile = _tiles[x, y];
                if (tile is null || tile.Animated) continue;
                var (tilesetX, tilesetY) = Tileset.GetTileCoordinates(tile.TileSetIndex);

                float rot = 0;
                #if DEBUG
                if (tile.RandomRotation && EnableRandomRotation)
                #else
                if (tile.RandomRotation)
                #endif
                {
                    rot = Utility.ChaosHash(x, y) % 4 * 90;
                }
                
                Tileset.RenderTile((x - chunkX) * Tileset.TileWidth, (y - chunkY) * Tileset.TileHeight, tilesetX,
                    tilesetY, rot);
            }
        }

        Raylib.EndTextureMode();
    }

    public void DestroyTile(int x, int y)
    {
        Tile? tile = _tiles[x, y];
        if (tile is null) return;

        Random rnd = new();
        int tilesheetX = (tile.TileSetIndex % Tileset.TileRows) * Tileset.TileWidth;
        int tilesheetY = (tile.TileSetIndex / Tileset.TileRows) * Tileset.TileHeight;
        for (int i = 0; i < rnd.Next(15, 20); i++)
        {
            Echoes.EchoesInstance.World.ParticleSystem.AddParticle("Content/Spritesheets/TerrainSpritesheet.png",
                new Vector2(x * Tileset.TileWidth + rnd.Next(Tileset.TileWidth), y * Tileset.TileHeight + rnd.Next(Tileset.TileHeight)), new Vector2(rnd.NextSingle() * 60 - 30, rnd.NextSingle() * 60 - 30), rnd.NextSingle() * 3 + 3, new Rectangle(tilesheetX + rnd.Next(0, Tileset.TileWidth - 3), tilesheetY + rnd.Next(0, Tileset.TileHeight - 3), 3, 3));
        }
        
        SetTile(x, y, null);
    }

    public void Clear()
    {
        _tiles = new Tile[Width, Height];
    }

    public void RerenderAll()
    {
        int chunkCount = (Width / ChunkSize) * (Height / ChunkSize);
        for (int i = 0; i < chunkCount; i++)
            RenderChunk(i);
        _dirtyChunks.Clear();
    }

    public void Dispose()
    {
        foreach (var texture in Chunks)
            Raylib.UnloadRenderTexture(texture);
    }

    public bool TileTouches(int x, int y, Tile tile)
    {
        if (_tiles[x + 1, y] == tile)
            return true;
        if (_tiles[x, y + 1] == tile)
            return true;
        if (_tiles[x - 1, y] == tile)
            return true;
        if (_tiles[x, y - 1] == tile)
            return true;

        return false;
    }

    public bool CheckCollision(Rectangle rect)
    {
        int x1 = (int) rect.X / Tileset.TileWidth;
        int y1 = (int) rect.Y / Tileset.TileHeight;
        int x2 = (int) (rect.X + rect.Width) / Tileset.TileWidth;
        int y2 = (int) (rect.Y + rect.Height) / Tileset.TileHeight;

        for (int y = y1; y <= y2; y++)
        {
            for (int x = x1; x <= x2; x++)
            {
                if (_tiles[x, y] is {IsSolid: true})
                    return true;
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int, int) WorldCoordToTileCoord(float x, float y) => ((int) (x / Tileset.TileWidth), (int) (y / Tileset.TileHeight));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int, int) WorldCoordToTileCoord(Vector2 pos) => ((int) (pos.X / Tileset.TileWidth), (int) (pos.Y / Tileset.TileHeight));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tile? TileAtTileCoord(int x, int y) => _tiles[x, y];
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tile? TileAtWorldCoord(float x, float y) => _tiles[(int) (x / Tileset.TileWidth), (int) (y / Tileset.TileHeight)];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Tile? TileAtWorldCoord(Vector2 pos) => TileAtWorldCoord(pos.X, pos.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetTileAtWorldCoord(float x, float y, Tile tile)
    {
        _tiles[(int) (x / Tileset.TileWidth), (int) (y / Tileset.TileHeight)] = tile;
        _dirtyChunks.Add(((int) (x / Tileset.TileWidth) / ChunkSize) + ((int) (y / Tileset.TileHeight) / ChunkSize) * (Width / ChunkSize));
    }
}
