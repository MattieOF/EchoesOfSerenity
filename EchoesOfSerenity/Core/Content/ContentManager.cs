using System.Diagnostics;
using Raylib_cs;

namespace EchoesOfSerenity.Core.Content;

public class ContentManager
{
    private static Dictionary<string, Texture2D> _textures = new();
    private static Dictionary<string, Font> _fonts = new();
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
            if (Path.GetFileName(file).StartsWith('_')) // TODO: ew
                continue;

#if DEBUG
            string loadType;
#endif
            var extension = Path.GetExtension(file);
            switch (extension)
            {
                case ".png":
#if DEBUG
                    loadType = "texture";
#endif
                    RegisterTexture(file, Raylib.LoadTexture(file));
                    break;
                case ".ttf":
#if DEBUG
                    loadType = "font";
#endif
                    var font = Raylib.LoadFontEx(file, 18, [], 250);
                    RegisterFont(file, font);
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

        foreach (var font in _fonts.Values)
            Raylib.UnloadFont(font);

        foreach (var sound in _sounds.Values)
            Raylib.UnloadSound(sound);
    }

    public static void RegisterTexture(string path, Texture2D texture)
    {
        path = path.Replace('\\', '/');
        _textures.Add(path, texture);
    }

    public static Texture2D GetTexture(string path) => _textures[path];

    public static void RegisterFont(string path, Font font)
    {
        path = path.Replace('\\', '/');
        _fonts.Add(path, font);
    }

    public static Font GetFont(string path) => _fonts[path];

    public static void RegisterSound(string path, Sound sound)
    {
        path = path.Replace('\\', '/');
        _sounds.Add(path, sound);
    }

    public static Sound GetSound(string path) => _sounds[path];
}
