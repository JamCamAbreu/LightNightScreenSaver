﻿using HPScreen.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNightScreenSaver.Entities
{
    public class CannonSuite
    {
        public List<Cannon> Cannons { get; set; }
        public enum SuiteLayer
        {
            Background,
            Foreground
        }
        public SuiteLayer Layer { get; set; }
        public CannonSuite(SuiteLayer layer)
        {
            Layer = layer;
            Cannons = new List<Cannon>();

            int numcannons = Ran.Current.Next(3, 7);
            int pad = 100;
            int space = (Graphics.Current.ScreenWidth - pad * 2) / numcannons;
            for (int i = 0; i < numcannons; i++)
            {
                Cannon can = new Cannon(layer);
                can.SetAbsolutePosition(pad + i * space, Graphics.Current.ScreenHeight - (int)can.SpriteHeight);
                Cannons.Add(can);
            }
        }
        public void Update()
        {
            foreach (Cannon c in Cannons)
            {
                c.Update();
            }
        }
        public void Draw()
        {
            Graphics.Current.SpriteB.Begin();
            foreach (Cannon c in Cannons)
            {
                c.Draw();
            }
            //Graphics.Current.DrawTrackedBallInfo();

            Graphics.Current.SpriteB.End();
        }
    }
}