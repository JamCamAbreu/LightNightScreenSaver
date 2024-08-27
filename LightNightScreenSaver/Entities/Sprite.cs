using HPScreen.Admin;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPScreen.Entities
{
    public abstract class Sprite
    {
        protected Texture2D Texture { get; set; }
        public string SpriteName
        {
            get
            {
                return Texture.Name;
            }
        }
        public float SpriteWidth
        {
            get
            {
                return Texture.Width;
            }
        }
        public float SpriteHeight
        {
            get
            {
                return Texture.Height;
            }
        }
        public float Scale { get; set; }
        public float TargetScale { get; set; }
        public bool Flipped { get; set; }
        public float Xpos { get; protected set; }
        public float centerx
        {
            get
            {
                return Xpos + (Texture.Width * Scale) / 2;
            }
        }
        public float centery
        {
            get
            {
                return Ypos + (Texture.Height * Scale) / 2;
            }
        }
        public float Ypos { get; protected set; }
        public Color? Highlight { get; set; }
        public Sprite()
        {
            string defaultsprite = "ball";
            SetSprite(defaultsprite);
            SetAbsolutePosition(0, 0);
            Scale = 1;
            TargetScale = 1;
            Flipped = false;
        }
        public Sprite(string sprite)
        {
            SetSprite(SpriteName);
            SetAbsolutePosition(0, 0);
            Scale = 1;
            TargetScale = 1;
            Flipped = false;
        }
        public abstract void Update();
        public virtual void Draw()
        {
            if (Texture == null)
            {
                throw new Exception("Did not initialize a sprite for this object");
            }

            //Graphics.Current.SpriteB.FillRectangle((int)Xpos, (int)Ypos, (int)(Texture.Width * Scale), (int)(Texture.Height * Scale), Color.Red);

            Rectangle destinationrect = new Rectangle((int)Xpos, (int)Ypos, (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));
            Rectangle? sourcerect = null;
            Color color = Highlight == null ? Color.White : (Color)Highlight;
            SpriteEffects seffects = Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            float rotation = 0;
            Vector2 origin = new Vector2(Texture.Width/2, Texture.Height/2);
            float layerdepth = 0;

            Graphics.Current.SpriteB.Draw(Texture, destinationrect, sourcerect, color, rotation, origin, seffects, layerdepth);
            //Graphics.Current.SpriteB.FillRectangle((int)Xpos, (int)Ypos, 5, 5, Color.Purple);
        }
        public void SetAbsolutePosition(float x, float y)
        {
            this.Xpos = x;
            this.Ypos = y;
        }
        public void SetSprite(string spritename)
        {
            Texture = Graphics.Current.SpritesByName[spritename];

        }
    }
}
