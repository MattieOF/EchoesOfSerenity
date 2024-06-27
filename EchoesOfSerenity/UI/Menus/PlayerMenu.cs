using EchoesOfSerenity.Core;
using EchoesOfSerenity.World.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.UI.Menus;

public class PlayerMenu : TabMenu
{
    public PlayerMenu(PlayerEntity playerEntity)
    {
        Game.Instance.IsPaused = true;
        Background = new Color(0, 0, 0, 200);
        
        Tabs = [
            ("Inventory", new InventoryMenu(playerEntity)),
            ("Achievements", new AchievementMenu(playerEntity)),
            ("Stats", new StatMenu(playerEntity))
        ];
    }

    public override void Update()
    {
        base.Update();
        
        if (Raylib.IsKeyPressed(KeyboardKey.Escape) || Raylib.IsKeyPressed(KeyboardKey.Tab))
        {
            Close();
        }
    }

    public void Close()
    {
        Tabs[ActiveTab].menu.OnClosed();
        Game.Instance.IsPaused = false;
        RemoveFromParent();
    }
}
