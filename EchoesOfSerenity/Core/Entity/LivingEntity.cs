using Raylib_cs;

namespace EchoesOfSerenity.Core.Entity;

public class LivingEntity : AnimatedEntity
{
    public float Health = 10;
    public float ImmunityTimer = 0;
    
    public override void Update()
    {
        if (ImmunityTimer >= 0)
            ImmunityTimer -= Raylib.GetFrameTime();
    }
    
    public void Hurt(float damage)
    {
        if (ImmunityTimer >= 0)
            return;
        
        Health -= damage;
        
        if (Health <= 0)
            Die();
        else
            ImmunityTimer = .3f;
    }

    public virtual void Die()
    {
        World.RemoveEntity(this);
    }
}
