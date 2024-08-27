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
    public class ParticleLayers
    {
        #region Singleton Implementation
        private static ParticleLayers instance;
        private static object _lock = new object();
        private ParticleLayers()
        {
        }
        public static ParticleLayers Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new ParticleLayers();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        public List<LifeParticleEffect> BackgroundEffects { get; set; } = new List<LifeParticleEffect>();
        public List<LifeParticleEffect> ForegroundEffects { get; set; } = new List<LifeParticleEffect>();
        public void AddBackgroundEffect(LifeParticleEffect effect)
        {
            BackgroundEffects.Add(effect);
        }
        public void AddForegroundEffect(LifeParticleEffect effect)
        {
            ForegroundEffects.Add(effect);
        }
        public void Update(GameTime gameTime)
        {
            // Clear expired background effects:
            List<LifeParticleEffect> deadEffects = new List<LifeParticleEffect>();
            foreach (LifeParticleEffect effect in BackgroundEffects)
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
                BackgroundEffects.Remove(effect);
            }

            // Clear expired foreground effects:
            deadEffects.Clear();
            foreach (LifeParticleEffect effect in ForegroundEffects)
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
                ForegroundEffects.Remove(effect);
            }
        }
        public void DrawBackgroundEffects(GameTime gameTime)
        {
            Graphics.Current.SpriteB.Begin();
            foreach (LifeParticleEffect effect in BackgroundEffects)
            {
                Graphics.Current.SpriteB.Draw(effect.ParticleEffect);
            }
            Graphics.Current.SpriteB.End();
        }
        public void DrawForegroundEffects(GameTime gameTime)
        {
            Graphics.Current.SpriteB.Begin();
            foreach (LifeParticleEffect effect in ForegroundEffects)
            {
                Graphics.Current.SpriteB.Draw(effect.ParticleEffect);
            }
            Graphics.Current.SpriteB.End();
        }
    }
}
