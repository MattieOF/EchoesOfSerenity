using EchoesOfSerenity.Core.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.World.Item;

public enum ToolType
{
    None,
    Axe,
    Pickaxe,
    Shovel
}

public class Item
{
    public string Name = "Item";
    public Texture2D Texture;
    public int MaxStack = 64;
    public bool Consumable = false;
    public ToolType ToolType = ToolType.None;
    public int ToolStrength = 0;

    public virtual bool OnUsed(LivingEntity user)
    {
        return false;
    }
}
