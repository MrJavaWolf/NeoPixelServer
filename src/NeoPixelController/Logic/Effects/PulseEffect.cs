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
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = nameof(PulseEffect);
        public bool IsEnabled { get; set; } = true;
        public int SkipPixels { get; set; }
        public int NumberOfPixels { get; set; }
        public IColorProvider ColorProvider { get; set; }
        public float EffectSpeed { get; set; }

        private readonly NeoPixelStrip strip;
        private float offset = 0;

        public PulseEffect(
            NeoPixelStrip strip,
            int skipPixels,
            int numberOfPixels,
            IColorProvider colorProvider,
            float speed)
        {
            this.strip = strip;
            this.SkipPixels = skipPixels;
            this.ColorProvider = colorProvider;
            this.NumberOfPixels = numberOfPixels;
            this.EffectSpeed = speed;
        }

        public void Enter(EffectTime time)
        {

        }

        public void Exit(EffectTime time)
        {

        }

        public void Update(EffectTime time)
        {
            for (int i = SkipPixels; i < strip.Pixels.Count && i < SkipPixels + NumberOfPixels; i++)
            {
                double rawCalculation = strip.Pixels.Count / (double)i;
                Color color = ColorProvider.GetColor(time);
                Color c = Color.FromArgb(
                    (byte)(color.R * rawCalculation),
                    (byte)(color.G * rawCalculation),
                    (byte)(color.B * rawCalculation));
                strip.Pixels[SkipPixels + (i + (int)offset) % NumberOfPixels] = 
                    strip.Pixels[SkipPixels + (i + (int)offset) % NumberOfPixels].Add(c);
            }

            offset += EffectSpeed * time.DeltaTime / 1000.0f;
        }

    }
}
