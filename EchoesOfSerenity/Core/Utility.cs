using System.Numerics;
using System.Runtime.CompilerServices;
using Raylib_cs;

namespace EchoesOfSerenity.Core;

public static class Utility
{
    public static void WriteLineColour(ConsoleColor color, string message)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = oldColor;
    }

    public static bool IsValidIndex<T>(this T[] array, int index)
    {
        return index >= 0 && index < array.Length;
    }

    public static bool IsValidIndexRO<T>(this IReadOnlyCollection<T> list, int index)
    {
        return index >= 0 && index < list.Count;
    }

    public static bool IsValidIndex<T>(this ICollection<T> list, int index)
    {
        return index >= 0 && index < list.Count;
    }
    
    // Thanks to https://stackoverflow.com/a/37498217
    private static int Seed2(int seed)
    {
        var s = 192837463 ^ System.Math.Abs(seed);
        var a = 1664525;
        var c = 1013904223;
        var m = 4294967296;
        return (int) ((s * a + c) % m);
    }

    public static int GetSeedXY(int x, int y)
    {
        int sx = Seed2(x * 1947);
        int sy = Seed2(y * 2904);
        return Seed2(sx ^ sy);
    }
    
    // Thanks to https://stackoverflow.com/a/37221804
    public static int ChaosHash(int x, int y, int seed = 0)
    {   
        int h = seed + x*374761393 + y*668265263;
        h = (h^(h >> 13))*1274126177;
        return h^(h >> 16);
    }

    // Thanks to Freya Holmer: https://www.youtube.com/watch?v=LSNQuFEDOyQ
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 LerpSmooth(Vector2 a, Vector2 b, float t = 0.05f)
    {
        return (a - b) * float.Pow(t, Raylib.GetFrameTime()) + b;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float LerpSmooth(float a, float b, float t = 0.05f)
    {
        return (a - b) * float.Pow(t, Raylib.GetFrameTime()) + b;
    }
    
    public static bool IsPointInRect(Vector2 point, Rectangle rect)
    {
        return point.X >= rect.X && point.X <= rect.X + rect.Width && point.Y >= rect.Y && point.Y <= rect.Y + rect.Height;
    }
}
