using Raylib_cs;

namespace EchoesOfSerenity.Core;

public class Animation(int startSprite, int length, int fps)
{
    public int StartSprite = startSprite;
    public int Length = length;
    public int FPS = fps;
}

public class Spritesheet(int spriteWidth = 16, int spriteHeight = 16)
{
    public Texture2D Texture;
    public Dictionary<string, Animation> Animations = new();

    public int SpriteWidth = spriteWidth, SpriteHeight = spriteHeight;

    public void SetTexture(Texture2D texture)
    {
        if (texture.Width % SpriteWidth != 0
            || texture.Height % SpriteHeight != 0)
        {
            Utility.WriteLineColour(ConsoleColor.Red, "Invalid texture for spritesheet");
            return;
        }

        Texture = texture;
    }
    
    public void AddAnimation(string name, int startSprite, int length, int fps)
    {
        Animations.Add(name, new Animation(startSprite, length, fps));
    }
}
