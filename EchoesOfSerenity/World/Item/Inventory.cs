namespace EchoesOfSerenity.World.Item;

public class Inventory
{
    public int Size { get; private set; } = 18;
    public static int RowSize = 6;
    public readonly List<(Item?, int)> Contents = [];
    
    public Inventory(int size = 18)
    {
        Size = size;
        Contents.Clear();
        for (int i = 0; i < Size; i++)
            Contents.Add((null, 0));
    }
}
