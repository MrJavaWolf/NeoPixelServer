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
        private readonly Color color;

        public StaticColorProvider(Color color)
        {
            this.color = color;
        }

        public Color GetColor(EffectTime time)
        {
            return color;
        }
    }
}
