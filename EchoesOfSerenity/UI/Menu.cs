using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.UI;

public delegate bool OnButtonPressed(Button button);

public enum HorizontalAlignment
{
    Left, 
    Center,
    Right
}

public enum VerticalAlignment
{
    Top,
    Middle,
    Bottom
}

public abstract class UIElement
{
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            Layout();
        }
    }

    public Vector2 Padding
    {
        get => _padding;
        set
        {
            _padding = value;
            Layout();
        }
    }

    public Menu? Parent = null!;

    public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Left;
    public VerticalAlignment VerticalAlignment = VerticalAlignment.Top;

    public HorizontalAlignment HorizontalAnchor = HorizontalAlignment.Left;
    public VerticalAlignment VerticalAnchor = VerticalAlignment.Top;

    protected Vector2 RenderPos;
    private Vector2 _position, _padding;

    public virtual void LayoutFromSize(Vector2 size)
    {
        RenderPos = Position;

        switch (HorizontalAlignment)
        {
            case HorizontalAlignment.Center:
                RenderPos.X -= (size.X + _padding.X) / 2;
                break;
            case HorizontalAlignment.Right:
                RenderPos.X -= size.X + _padding.X;
                break;
        }

        switch (VerticalAlignment)
        {
            case VerticalAlignment.Middle:
                RenderPos.Y += (size.Y + _padding.Y) / 2;
                break;
            case VerticalAlignment.Bottom:
                RenderPos.Y += size.Y + _padding.Y;
                break;
        }

        switch (HorizontalAnchor)
        {
            case HorizontalAlignment.Center:
                RenderPos.X = Raylib.GetScreenWidth() / 2.0f + RenderPos.X;
                break;
            case HorizontalAlignment.Right:
                RenderPos.X = Raylib.GetScreenWidth() + RenderPos.X;
                break;
        }

        switch (VerticalAnchor)
        {
            case VerticalAlignment.Middle:
                RenderPos.Y = Raylib.GetScreenHeight() / 2.0f - RenderPos.Y;
                break;
            case VerticalAlignment.Bottom:
                RenderPos.Y = Raylib.GetScreenHeight() - RenderPos.Y;
                break;
        }
    }
    
    public virtual void Layout() { }
    public virtual void Update() { }
    public virtual void Render() { }
    public virtual void PostRender() { }
}

public class Label : UIElement
{
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            Layout();
        }
    }

    public Font Font
    {
        get => _font;
        set
        {
            _font = value;
            Layout();
        }
    }

    public int FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            Layout();
        }
    }

    public Color Colour = Color.White;

    private Font _font;
    private int _fontSize = 18;
    private string _text = "";

    public override void Layout()
    {
        if (Parent is null) return;
        var size = Raylib.MeasureTextEx(_font, _text, _fontSize, 0);
        LayoutFromSize(size);
    }

    public override void Update()
    { }

    public override void Render()
    {
        Raylib.DrawTextEx(Font, Text, RenderPos, FontSize, 0, Colour);
    }
}

public class Button : UIElement
{
    public List<OnButtonPressed> OnPressed = [];

    public string Text
    {
        get => _text;
        set => SetText(value);
    }

    public Rectangle Rect;

    public Font Font
    {
        get => _font;
        set
        {
            _font = value;
            Layout();
        }
    }

    public int FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            Layout();
        }
    }

    public bool IsHovered = false;
    public String Tooltip = "";
    public int TooltipSize = 18;
    
    private Font _font;
    private int _fontSize = 18;
    private string _text = "Button";

    public void SetText(string newText, bool updateRect = true)
    {
        _text = newText;
        var size = Raylib.MeasureTextEx(Font, Text, FontSize, 0);
        Rect.Width = size.X + Padding.X;
        Rect.Height = size.Y + Padding.Y;
        Layout();
    }

    public override void Layout()
    {
        if (Parent is null) return;
        LayoutFromSize(new Vector2(Rect.Width, Rect.Height));
    }

    public override void Update()
    {
        IsHovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), new Rectangle(RenderPos, Rect.Size));

        if (IsHovered && Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            foreach (var action in OnPressed)
                action(this);
        }
    }
    
    public override void Render()
    {
        Raylib.DrawRectangle((int)RenderPos.X, (int)RenderPos.Y, (int)Rect.Width, (int)Rect.Height, IsHovered ? (Raylib.IsMouseButtonDown(MouseButton.Left) ? Color.DarkGray : Color.Gray) : Color.White);
        Raylib.DrawTextEx(_font, _text, new Vector2(RenderPos.X + Padding.X / 2, RenderPos.Y + Padding.Y / 2), FontSize, 0, Color.Black);
    }

    public override void PostRender()
    {
        if (IsHovered && !string.IsNullOrEmpty(Tooltip))
        {
            var mousePos = Raylib.GetMousePosition();
            var size = Raylib.MeasureTextEx(Font, Text, TooltipSize, 0);
            Raylib.DrawRectangle((int)(mousePos.X + 20), (int)mousePos.Y, (int)(size.X + 70), (int)(size.Y + 10), Color.White);
            Raylib.DrawTextEx(Font, Tooltip, new Vector2(mousePos.X + 30, mousePos.Y + 5), TooltipSize, 0, Color.Black);
        }
    }
}

public class ImageButton : Button
{
    public Texture2D Texture;
    
    public override void Render()
    {
        Raylib.DrawTexturePro(Texture, new Rectangle(0, 0, Texture.Width, Texture.Height), new Rectangle(RenderPos, Rect.Size), Vector2.Zero, 0, IsHovered ? (Raylib.IsMouseButtonDown(MouseButton.Left) ? Color.DarkGray : Color.Gray) : Color.White);
    }

    public override void PostRender()
    {
        if (IsHovered && !string.IsNullOrWhiteSpace(Tooltip))
        {
            var mousePos = Raylib.GetMousePosition();
            var size = Raylib.MeasureTextEx(Font, Tooltip, TooltipSize, 0);
            Raylib.DrawRectangle((int)(mousePos.X + 20), (int)mousePos.Y, (int)(size.X + 20), (int)(size.Y + 20), Color.White);
            Raylib.DrawTextEx(Font, Tooltip, new Vector2(mousePos.X + 30, mousePos.Y + 5), TooltipSize, 0, Color.Black);
        }
    }
}

public class Menu
{
    public static NPatchInfo FrameNPatch = new()
    {
        Bottom = 10,
        Left = 10,
        Right = 10,
        Top = 10,
        Source = new Rectangle(0, 0, 32, 32),
        Layout = NPatchLayout.NinePatch
    };
    
    public List<UIElement> UIElements = [];
    public MenuLayer? Parent = null;
    public Color Background = Color.Blank;
    public float OverallOpacity = 1f;

    public void AddElement(UIElement element)
    {
        element.Parent = this;
        element.Layout();
        UIElements.Add(element);
    }

    public void RemoveElement(UIElement element)
    {
        element.Parent = null;
        UIElements.Remove(element);
    }

    public void RemoveFromParent()
    {
        Parent?.Remove();
    }

    public void Layout()
    {
        foreach (var element in UIElements)
            element.Layout();
    }
    
    public virtual void Update()
    {
        foreach (var element in UIElements)
            element.Update();
    }

    public virtual void Render()
    {
        if (Background.A != 0)
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), Background);
        
        foreach (var element in UIElements)
            element.Render();
        
        foreach (var element in UIElements)
            element.PostRender();
    }
    
    public virtual void OnClosed()
    { }
}
