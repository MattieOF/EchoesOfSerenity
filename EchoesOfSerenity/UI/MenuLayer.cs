using EchoesOfSerenity.Core;

namespace EchoesOfSerenity.UI;

public class MenuLayer : ILayer
{
    public Menu Menu;

    public MenuLayer(Menu menu)
    {
        Menu = menu;
        Menu.Parent = this;
    }

    public void Update()
    {
        Menu.Update();
    }

    public void RenderUI()
    {
        Menu.Render();
    }

    public void OnWindowResized()
    {
        Menu.Layout();
    }

    public void Remove()
    {
        Game.Instance.DetachLayer(this);
    }
}
