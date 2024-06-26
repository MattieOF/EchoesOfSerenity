using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;

namespace EchoesOfSerenity.World.Item;

public class JakeVoodooDoll : Item
{
    public JakeVoodooDoll()
    {
        Name = "Jake Voodoo Doll";
    }
    
    public override void OnUsed()
    {
        SoundManager.PlaySound(ContentManager.GetSound("Content/Sounds/ow.wav"));
    }
}
