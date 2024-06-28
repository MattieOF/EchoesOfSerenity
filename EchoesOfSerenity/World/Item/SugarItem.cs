using EchoesOfSerenity.Core.Entity;
using EchoesOfSerenity.World.Entity;

namespace EchoesOfSerenity.World.Item;

public class SugarItem : Item
{
    public SugarItem()
    {
        MaxStack = 64;
        UseType = UseType.Consumable;
        Consumable = true;
    }
    
    public override bool OnUsed(LivingEntity user)
    {
        user.Health = Math.Clamp(user.Health + 1, 0, 10);
        if (user is PlayerEntity player)
        {
            player.Achievements.CompleteAchievement("eat_sugar");
            player.SpeedBuff = Math.Max(player.SpeedBuff, 1.5f);
            player.SpeedBuffTimer = Math.Clamp(player.SpeedBuffTimer + 10, 0, 40);
        }
        return true;
    }
}
