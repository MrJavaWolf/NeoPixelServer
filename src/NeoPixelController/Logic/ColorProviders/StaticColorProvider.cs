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

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = nameof(StaticColorProvider);
        public Color Color { get; set; }

        public StaticColorProvider(Color color)
        {
            this.Color = color;
        }

        public Color GetColor(EffectTime time)
        {
            return Color;
        }

        public void Update(EffectTime time)
        {
        }
    }
}
