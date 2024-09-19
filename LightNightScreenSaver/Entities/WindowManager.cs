using HPScreen.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNightScreenSaver.Entities
{
    public struct WindowConfiguration
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public int GapWidth { get; set; }
        public int GapHeight { get; set; }
    }
    public class WindowManager
    {
        public const int STANDARD_GAP_WIDTH = 50;
        public const int STANDARD_GAP_HEIGHT = 65;
        public List<WindowConfiguration> WindowConfigurations { get; set; }
        public List<Window> AllWindows { get; set; }

        public WindowManager()
        {
            WindowConfigurations = new List<WindowConfiguration>();
            AllWindows = new List<Window>();

            InitConfigurations();
            InitWindows();
        }

        public void InitConfigurations()
        {
            // 1st position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 254,
                Y = 1180,
                Columns = 3,
                Rows = 7,
                WindowWidth = 20,
                WindowHeight = 20,
                GapWidth = STANDARD_GAP_WIDTH,
                GapHeight = STANDARD_GAP_HEIGHT
            });

            // 2nd position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 619,
                Y = 1476,
                Columns = 3,
                Rows = 2,
                WindowWidth = 20,
                WindowHeight = 20,
                GapWidth = STANDARD_GAP_WIDTH,
                GapHeight = STANDARD_GAP_HEIGHT
            });

            // 3rd position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 910,
                Y = 1074,
                Columns = 3,
                Rows = 7,
                WindowWidth = 20,
                WindowHeight = 20,
                GapWidth = STANDARD_GAP_WIDTH,
                GapHeight = STANDARD_GAP_HEIGHT
            });

            // 4th position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 1616,
                Y = 1191,
                Columns = 3,
                Rows = 8,
                WindowWidth = 20,
                WindowHeight = 20,
                GapWidth = STANDARD_GAP_WIDTH,
                GapHeight = STANDARD_GAP_HEIGHT
            });

            // 5th position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 1908,
                Y = 1191,
                Columns = 3,
                Rows = 8,
                WindowWidth = 20,
                WindowHeight = 20,
                GapWidth = STANDARD_GAP_WIDTH,
                GapHeight = STANDARD_GAP_HEIGHT
            });

            // 6th position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 2412,
                Y = 876,
                Columns = 2,
                Rows = 4,
                WindowWidth = 15,
                WindowHeight = 15,
                GapWidth = 40,
                GapHeight = 50
            });

            // 7th position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 2386,
                Y = 1200,
                Columns = 3,
                Rows = 5,
                WindowWidth = 15,
                WindowHeight = 15,
                GapWidth = STANDARD_GAP_WIDTH,
                GapHeight = STANDARD_GAP_HEIGHT
            });

            // 8th position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 2319,
                Y = 1614,
                Columns = 6,
                Rows = 3,
                WindowWidth = 15,
                WindowHeight = 15,
                GapWidth = STANDARD_GAP_WIDTH,
                GapHeight = STANDARD_GAP_HEIGHT
            });

            // 9th position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 2965,
                Y = 1515,
                Columns = 2,
                Rows = 4,
                WindowWidth = 20,
                WindowHeight = 20,
                GapWidth = STANDARD_GAP_WIDTH,
                GapHeight = STANDARD_GAP_HEIGHT
            });

            // 10th position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 3166,
                Y = 1483,
                Columns = 2,
                Rows = 4,
                WindowWidth = 20,
                WindowHeight = 20,
                GapWidth = STANDARD_GAP_WIDTH,
                GapHeight = STANDARD_GAP_HEIGHT
            });

            // 11th position
            WindowConfigurations.Add(new WindowConfiguration()
            {
                X = 3450,
                Y = 1446,
                Columns = 4,
                Rows = 5,
                WindowWidth = 20,
                WindowHeight = 20,
                GapWidth = STANDARD_GAP_WIDTH,
                GapHeight = STANDARD_GAP_HEIGHT
            });
        }
        public void InitWindows()
        {
            foreach (WindowConfiguration windowConfiguration in WindowConfigurations)
            {
                for (int i = 0; i < windowConfiguration.Columns; i++)
                {
                    for (int j = 0; j < windowConfiguration.Rows; j++)
                    {
                        Window window = new Window();
                        int posx = windowConfiguration.X + i * (windowConfiguration.WindowWidth + windowConfiguration.GapWidth);
                        int posy = windowConfiguration.Y + j * (windowConfiguration.WindowHeight + windowConfiguration.GapHeight);
                        window.SetPosition(posx, posy);
                        window.Width = windowConfiguration.WindowWidth;
                        window.Height = windowConfiguration.WindowHeight;
                        window.RefreshRectangle();
                        AllWindows.Add(window);
                    }
                }
            }
        }

        public void Update()
        {
            foreach (Window window in AllWindows)
            {
                window.Update();
            }
        }

        public void Draw()
        {
            Graphics.Current.SpriteB.Begin();
            
            foreach (Window window in AllWindows)
            {
                window.Draw();
            }
            Graphics.Current.SpriteB.End();
        }
    }
}
