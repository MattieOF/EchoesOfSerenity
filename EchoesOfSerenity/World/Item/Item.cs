using EchoesOfSerenity.Core.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.World.Item;

public enum UseType
{
    None,
    Tool,
    Unconsumable,
    Consumable,
    Placeable
}

public enum ToolType
{
    None,
    Axe,
    Pickaxe,
    Shovel
}

public delegate void OnCrafted(LivingEntity crafter);
public delegate void OnUsed(LivingEntity user);
public delegate void OnPickedUp(Core.Entity.Entity entity, int count);

public class Item
{
    public string Name = "Item";
    public string Description = "";
    public Texture2D Texture;
    public int MaxStack = 64;
    public bool Consumable = false;
    public UseType UseType = UseType.None;
    public ToolType ToolType = ToolType.None;
    public int ToolStrength = 0;
    public float UseSpeed = 1.0f;
    public List<OnCrafted> OnCrafted = [];
    public List<OnPickedUp> OnPickedUp = [];
    public List<OnUsed> OnUse = [];

    public virtual bool OnUsed(LivingEntity user)
    {
        foreach (var action in OnUse)
            action(user);
        return false;
    }

    public virtual bool OnAltUsed(LivingEntity user)
    {
        return false;
    }
}
