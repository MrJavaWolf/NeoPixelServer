using NeoPixelController.Interface;
using NeoPixelController.Logic.Extension;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic.Effects
{
    public class FullColorEffect : INeoPixelEffect
    {

        private readonly NeoPixelStrip strip;
        public IColorProvider ColorProvider { get; set; }

        public FullColorEffect(NeoPixelStrip strip, IColorProvider colorProvider)
        {
            this.strip = strip;
            this.ColorProvider = colorProvider;
        }

        public void Enter(EffectTime time)
        {

        }

        public void Update(EffectTime time)
        {
            
            Color color = ColorProvider.GetColor(time);
            
            for (int i = 0; i < strip.Pixels.Count; i++)
            {
                strip.Pixels[i] = strip.Pixels[i].Add(color);
            }
        }

        public void Exit(EffectTime time)
        {

        }


    }
}
