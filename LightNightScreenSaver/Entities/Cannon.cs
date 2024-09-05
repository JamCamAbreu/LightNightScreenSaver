using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HPScreen.Admin;
using HPScreen.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static LightNightScreenSaver.Entities.CannonSuite;

namespace LightNightScreenSaver.Entities
{
    public class Cannon : Sprite
    {
        public SuiteLayer Layer { get; set; }
        public float Angle { get; set; }
        public float Power { get; set; }
        public int LoadTimer { get; set; }
        public int LoadTimerStartValue { get; set; }
        public int LoadTimerMin { get; set; } = 200;
        public int LoadTimerMax { get; set; } = 550;
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
        public Cannon(SuiteLayer layer)
        {
            Layer = layer;
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

            float colorfactor = 1f - (float)LoadTimer / LoadTimerStartValue;
            Highlight = Color.Lerp(new Color(20, 20, 20), Color.Red, colorfactor);
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
            Firework shot = new Firework(Layer, Power);
            shot.SetSprite("ball");
            shot.SetAbsolutePosition(FirePosX, FirePosY);
            if (Layer == SuiteLayer.Background)
            {
                shot.Highlight = Color.DarkBlue;
            }
            shot.Scale = 0.5f * shot.LayerScale;
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
            LoadTimerStartValue = totalframes;

            RandomizeShot();
        }

        public void RandomizeShot()
        {
            float angleVarience = (float)Math.PI / 16;
            float baseAngle = (float)-Math.PI / 2 + Ran.Current.Next(-angleVarience, angleVarience);

            // -0.5 to 0.0    left side needs to shoot more clockwise
            //  0.0 to 0.5  right side needs to shoot more counter-clockwise
            float pos = Xpos / Graphics.Current.ScreenWidth - 0.5f;
            float positionAdjustedAngle = baseAngle + (float)((Math.PI/4) * pos);
            
            //float adjustedForScreen = 
            Angle = positionAdjustedAngle;
            Power = Ran.Current.Next(100, 200);
        }
    }
}
