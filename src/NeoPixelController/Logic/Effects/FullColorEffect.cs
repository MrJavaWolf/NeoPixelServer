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
    public class FullColorEffect : INeoPixelEffect
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Description("The name of the effect")]
        public string Name { get; set; } = nameof(FullColorEffect);

        [DisplayName("Enable")]
        [Description("Enable/Disable the effect.")]
        public bool IsEnabled { get; set; } = true;

        [Description("How bright the effect is (0 = off, 1 = full brightness).")]
        public float Intensity { get; set; } = 1;

        public IColorProvider ColorProvider { get; set; }
        
        private readonly IEnumerable<NeoPixelDriver> drivers;

        public FullColorEffect(IEnumerable<NeoPixelDriver> drivers, IColorProvider colorProvider)
        {
            this.drivers = drivers;
            this.ColorProvider = colorProvider;
        }

        public void Enter(EffectTime time)
        {

        }

        public void Update(EffectTime time)
        {

            Color color = ColorProvider.GetColor(time);
            foreach (var driver in drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    for (int i = 0; i < strip.Pixels.Length; i++)
                    {
                        strip.Pixels[i] = strip.Pixels[i].Add(Color.FromArgb(
                            (byte)(color.R * Intensity),
                            (byte)(color.G * Intensity),
                            (byte)(color.B * Intensity)));
                    }
                }
            }
            ColorProvider.Update(time);
        }

        public void Exit(EffectTime time)
        {

        }


    }
}
