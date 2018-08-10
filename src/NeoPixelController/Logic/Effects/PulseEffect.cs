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
        public int AreaStartPosition { get; set; }
        public int AreaLength { get; set; }
        public IColorProvider ColorProvider { get; set; }
        public float EffectSpeed { get; set; }
        public float Intensity { get; set; } = 1;

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
            this.AreaStartPosition = skipPixels;
            this.ColorProvider = colorProvider;
            this.AreaLength = numberOfPixels;
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
            for (int i = AreaStartPosition; i < strip.Pixels.Count && i < AreaStartPosition + AreaLength; i++)
            {
                double rawCalculation = strip.Pixels.Count / (double)i;
                Color color = ColorProvider.GetColor(time);
                Color c = Color.FromArgb(
                    (byte)(color.R * rawCalculation * Intensity),
                    (byte)(color.G * rawCalculation * Intensity),
                    (byte)(color.B * rawCalculation * Intensity));
                strip.Pixels[AreaStartPosition + (i + (int)offset) % AreaLength] = 
                    strip.Pixels[AreaStartPosition + (i + (int)offset) % AreaLength].Add(c);
            }

            offset += EffectSpeed * time.DeltaTime / 1000.0f;
        }

    }
}
