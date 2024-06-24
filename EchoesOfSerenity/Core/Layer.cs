namespace EchoesOfSerenity.Core;

public class Layer
{
    public virtual void OnAttach() { }
    public virtual void OnDetach() { }
    public virtual void Update() { }
    public virtual void Render() { }
    public virtual void RenderImGUI() { }
}
