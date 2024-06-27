using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.World.Achievement;

public class AchievementNotificationLayer : ILayer
{
    public float Timer = 7f;
    
    private readonly Achievement _achievement;
    private Rectangle _rect;
    private Font _titleFont, _font;
    private Texture2D _trophy;
    private Vector2 _size;
    
    public AchievementNotificationLayer(Achievement achievement)
    {
        _achievement = achievement;

        _titleFont = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 30);
        _font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 18);
        _trophy = ContentManager.GetTexture("Content/UI/Trophy.png");

        Vector2 size = Vector2.Zero;
        void AddSize(Vector2 vec)
        {
            size.Y += vec.Y;
            size.X = MathF.Max(vec.X, size.X);
        }
        
        AddSize(Raylib.MeasureTextEx(_titleFont, achievement.Name, 30, 0));
        _size = Raylib.MeasureTextEx(_font, achievement.Description, 18, 0);
        AddSize(_size);

        size.X += _trophy.Width;
        size += new Vector2(60, 20);

        _rect = new Rectangle(50, 50, size);
    }

    public void Update()
    {
        Timer -= Raylib.GetFrameTime();
        if (Timer <= 0)
            Game.Instance.DetachLayer(this);
    }

    public void RenderUI()
    {
        var alpha = 1.0f;
        if (Timer is < 1f)
            alpha = Raymath.Lerp(0, 1, Timer);
        else if (Timer is > 6.5f)
            alpha = Raymath.Lerp(1, 0, (Timer - 6.5f) * 2);

        var trophyCol = new Color(255, 255, 255, (int)(alpha * 255));
        var col = new Color(20, 20, 20, (int)(alpha * 255));
        Raylib.DrawRectangleRec(_rect, new Color(255, 203, 0, (int)(alpha * 255)));
        Raylib.DrawTexture(_trophy, (int)(_rect.X + 20), (int)(_rect.Y + 10), trophyCol);
        Raylib.DrawTextEx(_titleFont, _achievement.Name, new Vector2(_rect.X + 30 + _trophy.Width, _rect.Y + 10), 30, 0, col);
        Raylib.DrawTextEx(_font, _achievement.Description, new Vector2(_rect.X + 30 + _trophy.Width, _rect.Y + 15 + _size.Y), 18, 0, col);
    }
}
