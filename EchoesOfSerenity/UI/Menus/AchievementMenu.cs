using System.Numerics;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.World.Achievement;
using EchoesOfSerenity.World.Entity;
using Raylib_cs;

namespace EchoesOfSerenity.UI.Menus;

public class AchievementMenu : Menu
{
    private PlayerEntity _player;
    private Font _font, _tooltipBoldFont, _tooltipFont, _headerFont;
    private Texture2D _frame, _greenFrame;
    private int _complete, _total;

    public AchievementMenu(PlayerEntity player)
    {
        _player = player;
        _font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 18);
        _tooltipBoldFont = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 18);
        _tooltipFont = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 18);
        _headerFont = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 40);
        _frame = ContentManager.GetTexture("Content/UI/Frame.png");
        _greenFrame = ContentManager.GetTexture("Content/UI/GreenFrame.png");

        _total = player.Achievements.AchievementList.Count;
        foreach (var achievement in player.Achievements.AchievementList.Values)
        {
            if (achievement.Completed)
                _complete++;
        }
    }

    public override void Render()
    {
        int x = 50, y = 150;

        Raylib.DrawTextEx(_headerFont, "Achievements", new Vector2(x, y), 40, 0, Color.White);
        
        y += 50;
        string completedText = $"Complete: {_complete}/{_total}";
        var completedTextSize = Raylib.MeasureTextEx(_font, completedText, 18, 0);
        Raylib.DrawTextEx(_font, completedText, new Vector2(x, y), 18, 0, Color.White);
        x = (int)(x + completedTextSize.X + 15);
        Raylib.DrawRectangle(x, y, Raylib.GetScreenWidth() - x - 100, (int)completedTextSize.Y, new Color(20, 20, 20, 255));
        Raylib.DrawRectangle(x, y, (int)((Raylib.GetScreenWidth() - x - 100) * ((float)_complete / _total)), (int)completedTextSize.Y, new Color(20, 255, 20, 255));

        var mousePos = Raylib.GetMousePosition();
        Achievement? hoveredAchievement = null;
        x = 50;
        y += 50;
        foreach (var (_, achievement) in _player.Achievements.AchievementList)
        {
            bool hovered = false;
            if (Raylib.CheckCollisionPointRec(mousePos, new Rectangle(x, y, 64, 64)))
            {
                hovered = true;
                hoveredAchievement = achievement;
            }
            Raylib.DrawTexturePro(achievement.Completed ? _greenFrame : _frame, new Rectangle(0, 0, _frame.Width, _frame.Height), new Rectangle(x, y, 64, 64), Vector2.Zero, 0, Color.White);
            Raylib.DrawTexturePro(achievement.Icon, new Rectangle(0, 0, achievement.Icon.Width, achievement.Icon.Height), new Rectangle(x + (hovered ? 6 : 12), y + (hovered ? 6 : 12), 64 - (hovered ? 12 : 24), 64 - (hovered ? 12 : 24)), Vector2.Zero, 0, Color.White);
            x += 68;

            if (x > Raylib.GetScreenWidth() - (50 + 64))
            {
                x = 50;
                y += 64 + 10;
            }
        }

        if (hoveredAchievement is not null)
        {
            Vector2 size = new(40, 20);
            List<(Font font, string text, Vector2 pos)> _queuedDraws = [];
            Vector2 pos = mousePos + new Vector2(40, 10);
            
            void DrawText(string text, Font font)
            {
                var sizeIncrease = string.IsNullOrWhiteSpace(text)
                    ? Vector2.Zero
                    : Raylib.MeasureTextEx(_tooltipFont, text, 18, 0);
                size.Y += sizeIncrease.Y;
                size.X = MathF.Max(size.X, sizeIncrease.X + 40);
                _queuedDraws.Add((font, text, pos));
                pos.Y += sizeIncrease.Y + 2;
            }
            
            DrawText(hoveredAchievement.Name, _tooltipBoldFont);
            DrawText(hoveredAchievement.Description, _tooltipFont);
            if (hoveredAchievement.StatGoal != 0)
            {
                Stat stat = _player.Stats.StatList[hoveredAchievement.StatID];
                if (stat.Rounded)
                {
                    DrawText($"Progress: {(int)stat.Value}/{(int)hoveredAchievement.StatGoal}", _tooltipFont);
                }
                else
                {
                    DrawText($"Progress: {stat.Value}/{hoveredAchievement.StatGoal}", _tooltipFont);
                }
            }
            
            Raylib.DrawTextureNPatch(_frame, FrameNPatch, new Rectangle(mousePos + new Vector2(20, 0), size), Vector2.Zero, 0, Color.White);
            foreach (var (font, text, textPos) in _queuedDraws)
                Raylib.DrawTextEx(font, text, textPos, 18, 0, Color.White);
        }
    }
}
