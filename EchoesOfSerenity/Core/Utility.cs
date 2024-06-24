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
}
