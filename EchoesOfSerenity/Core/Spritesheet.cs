using Raylib_cs;

namespace EchoesOfSerenity.Core;

public class Animation
{
    public string Name;
    public (int, int) StartSprite;
    public int Length;
    public int FPS;
    public Rectangle[] Frames;
    
    public Animation(string name, int startX, int startY, int spriteWidth, int spriteHeight, int length, int fps)
    {
        Name = name;
        StartSprite = (startX, startY);
        Length = length;
        FPS = fps;
        
        Frames = new Rectangle[length];
        for (int i = 0; i < length; i++)
            Frames[i] = new Rectangle((startX + i) * spriteWidth, startY * spriteHeight, spriteWidth, spriteHeight);
    }
}

public class Spritesheet(int spriteWidth = 16, int spriteHeight = 16)
{
    public Texture2D Texture;
    public Dictionary<string, Animation> Animations = new();

    public readonly int SpriteWidth = spriteWidth, SpriteHeight = spriteHeight;

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
    
    public void AddAnimation(string name, int startX, int startY, int length, int fps)
    {
        Animations.Add(name, new Animation(name, startX, startY, SpriteWidth, SpriteHeight, length, fps));
    }
}
