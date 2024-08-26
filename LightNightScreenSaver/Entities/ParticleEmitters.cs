using HPScreen.Admin;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNightScreenSaver.Entities
{
    public class ParticleEmitters
    {
        #region Singleton Implementation
        private static ParticleEmitters instance;
        private static object _lock = new object();
        private ParticleEmitters()
        {
        }
        public static ParticleEmitters Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new ParticleEmitters();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        public List<LifeParticleEffect> Effects { get; set; } = new List<LifeParticleEffect>();
        public void AddNewEffect(LifeParticleEffect effect)
        {
            Effects.Add(effect);
        }
        public void Update(GameTime gameTime)
        {
            List<LifeParticleEffect> deadEffects = new List<LifeParticleEffect>();
            foreach (LifeParticleEffect effect in Effects)
            {
                effect.Update(gameTime);
                if (effect.IsExpired())
                {
                    deadEffects.Add(effect);
                }
            }
            foreach (LifeParticleEffect effect in deadEffects)
            {
                effect.Dispose();
                Effects.Remove(effect);
            }
        }
        public void Draw(GameTime gameTime)
        {
            Graphics.Current.SpriteB.Begin();
            foreach (LifeParticleEffect effect in Effects)
            {
                Graphics.Current.SpriteB.Draw(effect.ParticleEffect);
            }
            Graphics.Current.SpriteB.End();
        }
    }
}
