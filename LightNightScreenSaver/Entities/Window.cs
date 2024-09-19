using HPScreen.Admin;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNightScreenSaver.Entities
{

    public class Window
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float Transparency { get; set; }
        public float TargetTransparency { get; set; }
        public Rectangle DrawnRectangle { get; set; }
        public Rectangle LightRectangle { get; set; }
        public Color LightColor { get; set; }
        public int Timer { get; set; }
        public const int TimerMin = 600;
        public const int TimerMax = 2000;
        public const int CooldownAmount = 300;
        public int CooldownTimer { get; set; }
        public void SetPosition(int x, int y)
        {
            X = (int)Graphics.GetStretchedPositionX(x);
            Y = (int)Graphics.GetStretchedPositionY(y);
            RefreshRectangle();
        }
        public Window()
        {
            Width = 20;
            Height = 32;
            Transparency = 0f;
            TargetTransparency = 0f;
            CooldownTimer = 0;
            ResetTimer();

            LightColor = Color.Lerp(Color.Yellow, Color.Orange, Ran.Current.Next(0.0f, 0.25f));
        }
        public void RefreshRectangle()
        {
           DrawnRectangle = new Rectangle(X, Y, Width, Height);
           LightRectangle = new Rectangle(X - 100, Y - 100, Width + 200, Height + 200);
        }
        public void ResetTimer()
        {
            Timer = Ran.Current.Next(TimerMin, TimerMax);
            Timer = (int)(Timer * (Graphics.MAX_DARKNESS - Graphics.Current.Darkness)) + CooldownTimer;
        }
        public void CheckLight()
        {
            if (Graphics.Current.Darkness < Graphics.DARKNESS_LIGHT_MIN)
            {
                if (TargetTransparency > 0) { TargetTransparency = 0; }
                return;
            }

            if (TargetTransparency == 0)
            {
                float percentToMidnight = (float)Graphics.Current.Darkness / (float)Graphics.MAX_DARKNESS;
                float chance = percentToMidnight - Ran.Current.Next(0.1f, 0.5f);
                if (Ran.Current.Next(0f, 1f) < chance)
                {
                    TargetTransparency = Ran.Current.Next(0.2f, 0.5f);
                    CooldownTimer += CooldownAmount;
                }
            }
            else
            {
                TargetTransparency = 0;
            }
        }
        public void Update()
        {
            Timer--;
            CooldownTimer--;
            if (CooldownTimer < 0) { CooldownTimer = 0; }
            if (Timer <= 0)
            {
                CheckLight();
                ResetTimer();
            }

            Transparency = Global.Ease(Transparency, TargetTransparency, 0.1f);
        }
        public void Draw()
        {
            Graphics.Current.SpriteB.FillRectangle(DrawnRectangle, LightColor * Transparency, 0.0f);
        }
    }
}
