using LightNightScreenSaver.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HPScreen.Admin
{
    public class Graphics
    {
        #region Singleton Implementation
        private static Graphics instance;
        private static object _lock = new object();
        private Graphics()
        {
            DebugFont = new Font(Color.White, Font.Type.arial, Font.Size.SIZE_M, true);
        }
        public static Graphics Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Graphics();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public void Init(GraphicsDevice gd, GameWindow window, bool fullscreen = false)
        {
            this.SpritesByName = new Dictionary<string, Texture2D>();
            this.Device = gd;
            this.Window = window;
            this.GraphicsDM.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this.GraphicsDM.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            this.GraphicsDM.IsFullScreen = true;
            this.GraphicsDM.ApplyChanges();
        }
        public Dictionary<string, Texture2D> SpritesByName { get; set; }
        public Dictionary<string, SpriteFont> Fonts { get; set; }
        public GraphicsDeviceManager GraphicsDM { get; set; }
        public SpriteBatch SpriteB { get; set; }
        public GraphicsDevice Device { get; set; }
        public GameWindow Window { get; set; }
        public int ScreenMidX { get { return Graphics.Current.Device.Viewport.Width / 2; } }
        public int ScreenMidY { get { return Graphics.Current.Device.Viewport.Height / 2; } }
        public int ScreenWidth { get { return Graphics.Current.Device.Viewport.Width; } }
        public int ScreenHeight { get { return Graphics.Current.Device.Viewport.Height; } }
        public int CenterStringX(int originX, string message, Font font)
        {
            if (string.IsNullOrEmpty(message)) { return originX; }
            if (string.IsNullOrEmpty(font.Name) || !this.Fonts.ContainsKey(font.Name)) { throw new Exception("Cannot find specified font"); }
            SpriteFont spritefont = this.Fonts[font.Name];
            return originX - (int)(spritefont.MeasureString(message).X / 2f);
        }
        public int CenterStringY(int originY, string message, Font font)
        {
            if (string.IsNullOrEmpty(font.Name) || !this.Fonts.ContainsKey(font.Name)) { throw new Exception("Cannot find specified font"); }
            SpriteFont spritefont = this.Fonts[font.Name];
            return originY - (int)(spritefont.MeasureString(message).Y / 2f);
        }
        public int RightAlignStringX(int originX, string message, Font font)
        {
            if (string.IsNullOrEmpty(message)) { return originX; }
            if (string.IsNullOrEmpty(font.Name) || !this.Fonts.ContainsKey(font.Name)) { throw new Exception("Cannot find specified font"); }
            SpriteFont spritefont = this.Fonts[font.Name];
            return originX + (int)(spritefont.MeasureString(message).X / 2f);
        }
        public int StringWidth(string message, Font font)
        {
            if (string.IsNullOrEmpty(message)) { return 0; }
            if (string.IsNullOrEmpty(font.Name) || !this.Fonts.ContainsKey(font.Name)) { throw new Exception("Cannot find specified font"); }
            SpriteFont spritefont = this.Fonts[font.Name];
            return (int)spritefont.MeasureString(message).X;
        }
        public int StringHeight(string message, Font font)
        {
            if (string.IsNullOrEmpty(message)) { return 0; }
            if (string.IsNullOrEmpty(font.Name) || !this.Fonts.ContainsKey(font.Name)) { throw new Exception("Cannot find specified font"); }
            SpriteFont spritefont = this.Fonts[font.Name];
            return (int)spritefont.MeasureString(message).Y;
        }
        public void DrawString(string text, Vector2 position, Font? font = null, bool centerX = false, bool centerY = false, bool callDrawBegin = false, Color? overrideColor = null)
        {
            Color color = overrideColor ?? font.Color;

            int posx = (int)position.X;
            if (centerX)
            {
                posx = this.CenterStringX(posx, text, font);
            }
            int posy = (int)position.Y;
            if (centerY)
            {
                posy = this.CenterStringY(posy, text, font);
            }

            if (callDrawBegin)
            {
                Graphics.Current.SpriteB.Begin();
            }

            if (font.Shadow)
            {
                SpriteFont sf = Graphics.Current.Fonts[font.Name];
                Color shadowcolor = new Color(Color.Black, color.A);
                for (int i = -2; i < 8; i++)
                {
                    Graphics.Current.SpriteB.DrawString(sf, text, new Vector2(posx + i, posy + i), shadowcolor);
                }
            }
            Graphics.Current.SpriteB.DrawString(Graphics.Current.Fonts[font.Name], text, new Vector2(posx, posy), color);

            if (callDrawBegin)
            {
                Graphics.Current.SpriteB.End();
            }
        }

        #region LightNight
        public Font DebugFont { get; set; }
        public GravityObject trackedball { get; set; }
        public void DrawTrackedBallInfo()
        {
            if (trackedball != null)
            {
                string message = 
                    $"X: {trackedball.Xpos}, Y: {trackedball.Ypos}\n" + 
                    $"Vx: {trackedball.XVelocity}, Vy: {trackedball.YVelocity}\n" +
                    $"Ax: {trackedball.XAcceleration}, Ay: {trackedball.YAcceleration}\n" +
                    $"Grav: {trackedball.Gravity}\n" + 
                    $"AirR: {trackedball.AirResistance}\n" +
                    $"Mass: {trackedball.Mass}";
                Vector2 pos = new Vector2(ScreenMidX, 50);

                this.DrawString(message, pos, DebugFont, true, false, false, null);
            }
        }
        #endregion
    }
}
