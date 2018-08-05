using NeoPixelController.Interface;
using NeoPixelController.Logic.Extension;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic.Effects
{
    class PulseEffect : INeoPixelEffect
    {
        private readonly NeoPixelStrip strip;
        private readonly int skipPixels;
        private readonly int numberOfPixels;
        private readonly IColorProvider colorProvider;

        //Pixels per second
        private readonly float effectSpeed = 25;
        private float offset = 0;

        public PulseEffect(
            NeoPixelStrip strip,
            int skipPixels,
            int numberOfPixels,
            IColorProvider colorProvider,
            float speed)
        {
            this.strip = strip;
            this.skipPixels = skipPixels;
            this.colorProvider = colorProvider;
            this.numberOfPixels = numberOfPixels;
            this.effectSpeed = speed;
        }

        public void Enter(EffectTime time)
        {

        }

        public void Exit(EffectTime time)
        {

        }

        public void Update(EffectTime time)
        {
            for (int i = skipPixels; i < strip.Pixels.Count && i < skipPixels + numberOfPixels; i++)
            {
                double rawCalculation = strip.Pixels.Count / (double)i;
                Color color = colorProvider.GetColor(time);
                Color c = Color.FromArgb(
                    (byte)(color.R * rawCalculation),
                    (byte)(color.G * rawCalculation),
                    (byte)(color.B * rawCalculation));
                strip.Pixels[skipPixels + (i + (int)offset) % numberOfPixels] = 
                    strip.Pixels[skipPixels + (i + (int)offset) % numberOfPixels].Add(c);
            }

            offset += effectSpeed * time.DeltaTime / 1000.0f;
        }

    }
}
