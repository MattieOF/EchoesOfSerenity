using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.Core.Entity;

namespace EchoesOfSerenity.World.Item;

public class JakeVoodooDollItem : Item
{
    public JakeVoodooDollItem()
    {
        Name = "Jake Voodoo Doll";
        MaxStack = 1;
        UseType = UseType.Unconsumable;
        Texture = ContentManager.GetTexture("Content/Items/Jake.png");
    }
    
    public override bool OnUsed(LivingEntity user)
    {
        SoundManager.PlaySound(ContentManager.GetSound("Content/Sounds/ow.wav"));
        return true;
    }
}
