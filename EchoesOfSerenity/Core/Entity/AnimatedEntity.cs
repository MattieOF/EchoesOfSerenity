using System.Numerics;
using Raylib_cs;

namespace EchoesOfSerenity.Core.Entity;

public class AnimatedEntity : Entity
{
    public Spritesheet Spritesheet = null!;
    public Animation CurrentAnimation = null!;
    public int CurrentFrame = 0;
    public float FrameTimer = 0;
    public float TargetRot = 0;
    public bool EnableRotLerp = true;
    
    protected float Rot = 0;

    public override void Render()
    {
        if (!Raylib.CheckCollisionRecs(Game.Instance.CameraBounds, BoundingBox))
            return;

        if (CurrentAnimation.Length > 1)
        {
            FrameTimer -= Raylib.GetFrameTime();
            if (FrameTimer <= 0)
            {
                FrameTimer = 1f / CurrentAnimation.FPS;
                CurrentFrame++;
                if (CurrentFrame >= CurrentAnimation.Length)
                    CurrentFrame = 0;
            }
        }

        if (EnableRotLerp)
            Rot = Utility.LerpSmooth(Rot, TargetRot, 0.03f);
        
        Rectangle frame = CurrentAnimation.Frames[CurrentFrame];
        Rectangle dest = new(Position.X + Size.X / 2, Position.Y + Size.Y / 2, frame.Width, frame.Height);
        Vector2 offset = new( frame.Width / 2,  frame.Height / 2);
        Raylib.DrawTexturePro(Spritesheet.Texture, frame, dest, offset, Rot, Color.White);
        
        base.Render();
    }

    public void SetAnimation(string name)
    {
        Animation anim = Spritesheet.Animations[name];
        if (CurrentAnimation == anim) return;
        CurrentFrame = 0;
        FrameTimer = 1f / anim.FPS;
        CurrentAnimation = anim;
    }
}
