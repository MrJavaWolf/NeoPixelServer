using NeoPixelController.Interface;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic.ColorProviders
{
    public class StaticColorProvider : IColorProvider
    {
        public Color Color { get; set; }

        public StaticColorProvider(Color color)
        {
            this.Color = color;
        }

        public Color GetColor(EffectTime time)
        {
            return Color;
        }
    }
}
