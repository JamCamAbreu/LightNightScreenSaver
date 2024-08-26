using HPScreen.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNightScreenSaver.Entities
{
    public class GameScreen
    {
        public List<Cannon> Cannons { get; set; }
        public GameScreen()
        {
            Cannons = new List<Cannon>();

            int numcannons = Ran.Current.Next(14, 22);
            for (int i = 0; i < numcannons; i++)
            {
                Cannon can = new Cannon();
                int minX = 100;
                int maxX = Graphics.Current.ScreenWidth - (int)can.SpriteWidth - 100;
                int minY = Graphics.Current.ScreenHeight - (int)can.SpriteHeight;
                int maxY = Graphics.Current.ScreenHeight - (int)can.SpriteHeight;
                can.SetAbsolutePosition(Ran.Current.Next(minX, maxX), Ran.Current.Next(minY, maxY));
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
