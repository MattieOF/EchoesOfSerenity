using Raylib_cs;

namespace EchoesOfSerenity.Core.Entity;

public class Mob : Entity
{
    public float Health = 100;
    public Spritesheet Spritesheet = null!;
    public Animation CurrentAnimation = null!;
    public int CurrentFrame = 0;
    public float FrameTimer = 0;

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
    }

    public void SetAnimation(string name)
    {
        Animation anim = Spritesheet.Animations[name];
        CurrentFrame = 0;
        FrameTimer = 1f / anim.FPS;
        CurrentAnimation = anim;
    }
}
