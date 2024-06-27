using System.Diagnostics;
using Raylib_cs;

namespace EchoesOfSerenity.Core.Content;

public class ContentManager
{
    private static Dictionary<string, Texture2D> _textures = new();
    private static Dictionary<string, Image> _images = new();
    private static Dictionary<string, Dictionary<int, Font>> _fonts = new();
    private static Dictionary<string, Sound> _sounds = new();

    public static void LoadContent()
    {
#if DEBUG
        Console.WriteLine("Loading content:");
#endif

        Stopwatch sw = new();
        sw.Start();

        var allFiles = Directory.GetFiles("Content/", "*.*", SearchOption.AllDirectories);
        foreach (var file in allFiles)
        {
            char first = Path.GetFileName(file)[0];
            if (first == '_') // TODO: ew
                continue;

#if DEBUG
            string loadType;
#endif
            var extension = Path.GetExtension(file);
            switch (extension)
            {
                case ".png":
                    bool isImage = Path.GetFileName(file).StartsWith("IMG");
#if DEBUG
                    loadType = isImage ? "image" : "texture";
#endif
                    if (isImage)
                        RegisterImage(file, Raylib.LoadImage(file));
                    else
                        RegisterTexture(file, Raylib.LoadTexture(file));
                    break;
                case ".ttf":
#if DEBUG
                    loadType = "font";
#endif
                    RegisterFont(file);
                    break;
                case ".wav":
#if DEBUG
                    loadType = "sound";
#endif
                    var sound = Raylib.LoadSound(file);
                    RegisterSound(file, sound);
                    break;
                default:
                    Utility.WriteLineColour(ConsoleColor.Red, $"Asset of unknown type: {extension}");
                    continue;
            }

#if DEBUG
            Utility.WriteLineColour(ConsoleColor.Green, $"\tLoading {loadType}: {file}");
#endif
        }

        sw.Stop();
        Console.WriteLine($"Loaded content in {sw.Elapsed.TotalMilliseconds:F}ms");
    }

    public static void UnloadAssets()
    {
        foreach (var texture in _textures.Values)
            Raylib.UnloadTexture(texture);
        
        foreach (var image in _images.Values)
            Raylib.UnloadImage(image);

        foreach (var fonts in _fonts.Values)
        {
            foreach (var font in fonts.Values)
            {
                Raylib.UnloadFont(font);
            }
        }

        foreach (var sound in _sounds.Values)
            Raylib.UnloadSound(sound);
    }

    public static void RegisterTexture(string path, Texture2D texture)
    {
        path = path.Replace('\\', '/');
        _textures.Add(path, texture);
    }

    public static Texture2D GetTexture(string path) => _textures[path];

    public static void RegisterImage(string path, Image image)
    {
        path = path.Replace('\\', '/');
        _images.Add(path, image);
    }

    public static Image GetImage(string path) => _images[path];

    public static void RegisterFont(string path)
    {
        path = path.Replace('\\', '/');
        _fonts.Add(path, []);
    }

    public static Font GetFont(string path, int size)
    {
        var sizes = _fonts[path];
        if (!sizes.ContainsKey(size))
        {
            sizes.Add(size, Raylib.LoadFontEx(path, size, [], 250));
        }

        return sizes[size];
    }

    public static void RegisterSound(string path, Sound sound)
    {
        path = path.Replace('\\', '/');
        _sounds.Add(path, sound);
    }

    public static Sound GetSound(string path) => _sounds[path];
}
