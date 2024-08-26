using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HPScreen.Admin;
using HPScreen.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace LightNightScreenSaver.Entities
{
    public class Cannon : Sprite
    {
        public float Angle { get; set; }
        public float Power { get; set; }
        public int LoadTimer { get; set; }
        public int LoadTimerMin { get; set; } = 45;
        public int LoadTimerMax { get; set; } = 160;
        public float FirePosX
        {
            get
            {
                return (Xpos);
            }
        }
        public float FirePosY
        {
            get
            {
                return Ypos - Texture.Height/2;
            }
        }
        protected List<Firework> Shots { get; set; }
        public Cannon()
        {
            SetSprite("cannon");
            Shots = new List<Firework>();
            Xpos = 0;
            Ypos = 0;
            Reload();
        }
        public override void Update()
        {
            LoadTimer--;
            if (LoadTimer <= 0)
            {
                Fire();
                Reload();
            }
            foreach (GravityObject go in Shots)
            {
                go.Update();
            }

            List<Firework> deadfireworks = new List<Firework>();
            foreach (Firework go in Shots)
            {
                if (go.IsDead)
                {
                    deadfireworks.Add(go);
                }
            }
            foreach (Firework go in deadfireworks)
            {
                go.Explode();
                Shots.Remove(go);
            }
        }
        public override void Draw()
        {
            base.Draw();
            foreach (Firework go in Shots)
            {
                go.Draw();
            }
        }
        public void Fire()
        {
            Firework shot = new Firework();
            shot.SetSprite("ball");
            shot.SetAbsolutePosition(FirePosX, FirePosY);
            shot.Scale = 0.5f;
            float xveloc = (float)Math.Cos(Angle) * Power;
            float yveloc = (float)Math.Sin(Angle) * Power;
            shot.ApplyForce(xveloc, yveloc);
            Shots.Add(shot);
            Graphics.Current.trackedball = shot;
            Reload();
        }
        public void Reload()
        {
            int totalframes = Ran.Current.Next(LoadTimerMin, LoadTimerMax);
            LoadTimer = totalframes;

            RandomizeShot();
        }

        public void RandomizeShot()
        {
            float angleVarience = (float)Math.PI / 4;
            Angle = (float)-Math.PI / 2 + Ran.Current.Next(-angleVarience, angleVarience);
            Power = Ran.Current.Next(60, 160);
        }
    }
}
