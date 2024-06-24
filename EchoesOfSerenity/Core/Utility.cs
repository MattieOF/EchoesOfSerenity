namespace EchoesOfSerenity.Core;

public class Utility
{
    public static void WriteLineColour(ConsoleColor color, string message)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = oldColor;
    }
}
