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
}
