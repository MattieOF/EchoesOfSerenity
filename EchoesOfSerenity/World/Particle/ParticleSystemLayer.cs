using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.World.Particle;

public struct Particle
{
    public int TextureIndex;
    public Rectangle TextureRect;
    public Vector2 Position;
    public Vector2 Velocity;
    public float Life;
    public bool EnableTransparency;

    public int Frames;
    public int FrameWidth;
    public float FrameTime;
    public float RemainingFrameTime;
}

public class ParticleSystemLayer : ILayer
{
    private readonly List<Particle> _particles = new();
    private readonly List<Texture2D> _textures = new();
    private readonly Dictionary<string, int> _textureListIndexes = new();
    
    public void AddParticle(string texture, Vector2 pos, Vector2 velocity, float life, Rectangle texRect, int frames = 0, int frameWidth = 16, float frameTime = 1, bool transparency = true)
    {
        int tex = 0;
        if (_textureListIndexes.ContainsKey(texture))
            tex = _textureListIndexes[texture];
        else
        {
            _textures.Add(ContentManager.GetTexture(texture));
            tex = _textures.Count - 1;
            _textureListIndexes.Add(texture, tex);
        }

        if (texRect.Width == 0)
        {
            texRect.Width = _textures[tex].Width;
            texRect.Height = _textures[tex].Height;
        }
        
        Particle particle = new()
        {
            TextureIndex = tex,
            TextureRect = texRect,
            Position = pos,
            Velocity = velocity,
            Life = life,
            Frames = frames,
            FrameWidth = frameWidth,
            FrameTime = frameTime,
            RemainingFrameTime = frameTime,
            EnableTransparency = transparency
        };
        _particles.Add(particle);
    }
    
    public void Render()
    {
        // Particles are visual only: since there is no multi-threading in this game, we can update and render particles in the same loop.

        float delta = Raylib.GetFrameTime();
        
        for (int i = _particles.Count - 1; i >= 0; i--)
        {
            Particle particle = _particles[i];

            if (!Utility.IsPointInRect(particle.Position, Game.Instance.CameraBounds))
            {
                _particles.RemoveAt(i);
                continue;
            }
            
            if (particle.Life > 0)
            {
                particle.Life -= delta;
                if (particle.Life <= 0)
                {
                    _particles.RemoveAt(i);
                    continue;
                }
                
                if (particle.Frames > 0)
                {
                    particle.RemainingFrameTime -= delta;
                    if (particle.RemainingFrameTime <= 0)
                    {
                        particle.TextureRect.X += particle.FrameWidth;
                        if (particle.TextureRect.X >= particle.FrameWidth * particle.Frames)
                            particle.TextureRect.X -= particle.FrameWidth * particle.Frames;
                        particle.RemainingFrameTime = particle.FrameTime;
                    }
                }

                particle.Position += particle.Velocity * delta;
                particle.Velocity -= particle.Velocity * delta * 5;
                
                Raylib.DrawTextureRec(_textures[particle.TextureIndex], particle.TextureRect, particle.Position, new Color(255, 255, 255, particle is { EnableTransparency: true, Life: > 0 and < 1 } ? (byte) Raymath.Lerp(0, 255, particle.Life) : 255));
            }

            _particles[i] = particle;
        }
    }
}
