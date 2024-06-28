using EchoesOfSerenity.Core.Entity;

namespace EchoesOfSerenity.World.Item;

public class FoodItem : Item
{
    public int Heal;
    
    public FoodItem(int heal)
    {
        Heal = heal;
        Consumable = true;
        UseType = UseType.Consumable;
        MaxStack = 64;
    }
    
    public override bool OnUsed(LivingEntity user)
    {
        base.OnUsed(user);
        user.Health = Math.Clamp(user.Health + Heal, 0, 10);
        return true;
    }
}
