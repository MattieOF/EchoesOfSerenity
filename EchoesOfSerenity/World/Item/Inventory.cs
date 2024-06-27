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

    public bool CanPickUp(Item item)
    {
        for (int i = 0; i < Size; i++)
        {
            if (Contents[i].Item1 == null)
                return true;
            if (Contents[i].Item1 == item && Contents[i].Item2 < item.MaxStack)
                return true;
        }

        return false;
    }

    public int AddItem(Item item, int count)
    {
        for (int i = 0; i < Size; i++)
        {
            if (Contents[i].Item1 == null)
            {
                Contents[i] = (item, count);
                return 0;
            }
            if (Contents[i].Item1 == item && Contents[i].Item2 < item.MaxStack)
            {
                int space = item.MaxStack - Contents[i].Item2;
                if (space >= count)
                {
                    Contents[i] = (item, Contents[i].Item2 + count);
                    return 0;
                }

                Contents[i] = (item, item.MaxStack);
                count -= space;
            }
        }

        return count;
    }

    public void Empty()
    {
        for (int i = 0; i < Size; i++)
            Contents[i] = (null, 0);
    }
}
