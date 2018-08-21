using NeoPixelController.Interface;
using NeoPixelController.Logic.Extension;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic.Effects
{
    class PulseEffect : INeoPixelEffect
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Description("The name of the effect")]
        public string Name { get; set; } = nameof(PulseEffect);

        [DisplayName("Enable")]
        [Description("Enable/Disable the effect.")]
        public bool IsEnabled { get; set; } = true;

        [Description("How bright the effect is (0 = off, 1 = full brightness).")]
        public float Intensity { get; set; } = 1;

        [DisplayName("Area Start Position")]
        [Description("How far in the effect should start.")]
        public int AreaStartPosition { get; set; }

        [DisplayName("Area Length")]
        [Description("How long the effect area should be.")]
        public int AreaLength { get; set; }

        [Description("How fast the effect is.")]
        public float Speed { get; set; }

        public IColorProvider ColorProvider { get; set; }

        private readonly NeoPixelSetup neoPixelSetup;
        private float offset = 0;

        public PulseEffect(
            NeoPixelSetup neoPixelSetup,
            int skipPixels,
            int numberOfPixels,
            IColorProvider colorProvider,
            float speed)
        {
            this.neoPixelSetup = neoPixelSetup;
            this.AreaStartPosition = skipPixels;
            this.ColorProvider = colorProvider;
            this.AreaLength = numberOfPixels;
            this.Speed = speed;
        }

        public void Enter(EffectTime time)
        {

        }

        public void Exit(EffectTime time)
        {

        }

        public void Update(EffectTime time)
        {
            foreach (var driver in neoPixelSetup.Drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    for (int i = AreaStartPosition; i < strip.Pixels.Length && i < AreaStartPosition + AreaLength; i++)
                    {
                        double rawCalculation = strip.Pixels.Length / (double)i;
                        Color color = ColorProvider.GetColor(time);
                        Color c = Color.FromArgb(
                            (byte)(color.R * rawCalculation * Intensity),
                            (byte)(color.G * rawCalculation * Intensity),
                            (byte)(color.B * rawCalculation * Intensity));
                        strip.Pixels[AreaStartPosition + (i + (int)offset) % AreaLength] =
                            strip.Pixels[AreaStartPosition + (i + (int)offset) % AreaLength].Add(c);
                    }
                }
            }
            offset += Speed * time.DeltaTime / 1000.0f;
        }


    }
}
