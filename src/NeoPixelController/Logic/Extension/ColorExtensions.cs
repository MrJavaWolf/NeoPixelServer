using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic.Extension
{
    public static class ColorExtensions
    {
        public static Color Add(this Color color, Color color2)
        {
            return Color.FromArgb(
                        (byte)Math.Min(255, (color.R + color2.R)),
                        (byte)Math.Min(255, color.G + color2.G),
                        (byte)Math.Min(255, color.B + color2.B));
        }
    }
}
