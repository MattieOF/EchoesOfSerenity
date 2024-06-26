using Raylib_cs;

namespace EchoesOfSerenity.World.Item;

public class Item
{
    public string Name = "Item";
    public Texture2D Texture;
    public int MaxStack = 64;

    public virtual void OnUsed()
    {
        
    }
}
