using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPScreen.Admin
{
    public class Font
    {
        public static List<int> Sizes = new List<int> { 48, 72, 96, 144 };
        public enum Size
        {
            SIZE_S = 48,
            SIZE_M = 72,
            SIZE_L = 96,
            SIZE_XL = 144
        }
        public enum Type
        {
            arial
        }
        public Font(Color color, Type fontName = Type.arial, Size fontSize = Size.SIZE_S, bool shadow = false)
        {
            Color = color;
            TargetColor = color;
            SelectedSize = (int)fontSize;
            TargetSize = (int)fontSize;
            FontName = fontName;
            Shadow = shadow;
        }
        public Color Color { get; set; }
        public Color TargetColor { get; set; }
        public int SelectedSize { get; set; }
        public int TargetSize { get; set; }
        public Type FontName { get; set; }
        public bool Shadow { get; set; }
        public string Name
        {
            get
            {
                return retrieveFontName();
            }
        }
        private string retrieveFontName()
        {
            string basename = "error";
            switch (FontName)
            {
                case Type.arial:
                    basename = "arial";
                    break;

                default:
                    throw new Exception("Please provide the file name of your Font here in this switch case");
            }

            return $"{basename}-{SelectedSize}";
        }
        public void IncreaseFontSize()
        {
            SelectedSize++;
            while (!Sizes.Contains(SelectedSize))
            {
                SelectedSize++;
                // Clamp to highest font size:
                if (SelectedSize >= Sizes[Sizes.Count - 1])
                {
                    SelectedSize = Sizes[Sizes.Count - 1];
                }
            }
        }
        public void DecreaseFontSize()
        {
            SelectedSize--;
            while (!Sizes.Contains(SelectedSize))
            {
                SelectedSize--;
                // Clamp to lowest font size:
                if (SelectedSize <= Sizes[0])
                {
                    SelectedSize = Sizes[0];
                }
            }

        }
        public void SetFontSize(Size size)
        {
            SelectedSize = (int)size;
            TargetSize = (int)size;
        }
        public Size GetFontSize()
        {
            return (Size)Enum.Parse(typeof(Size), this.SelectedSize.ToString());
        }
        public void SetColor(Color color)
        {
            Color = color;
            TargetColor = color;
        }
        public Font Copy()
        {
            return new Font(this.Color, this.FontName, (Font.Size)this.SelectedSize, this.Shadow);
        }

    }
}
