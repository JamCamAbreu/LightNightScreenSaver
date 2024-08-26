using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNightScreenSaver.Entities
{
    public class LifeParticleEffect
    {
        public ParticleEffect ParticleEffect { get; }
        public int LifetimeFrames { get; private set; }

        public LifeParticleEffect(ParticleEffect particleEffect, int lifetime)
        {
            ParticleEffect = particleEffect;
            LifetimeFrames = lifetime;
        }

        public void Update(GameTime gameTime)
        {
            LifetimeFrames--; // Decrease the lifetime by 1 frame
            ParticleEffect.Update(gameTime.GetElapsedSeconds());
        }

        public bool IsExpired()
        {
            return LifetimeFrames <= 0;
        }

        public void Dispose()
        {
            ParticleEffect.Dispose();
        }
    }
}
