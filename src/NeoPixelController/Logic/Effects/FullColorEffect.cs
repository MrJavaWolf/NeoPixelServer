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
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = nameof(FullColorEffect);
        public bool IsEnabled { get; set; } = true;
        public IColorProvider ColorProvider { get; set; }
        public float Intensity { get; set; } = 1;
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
                    for (int i = 0; i < strip.Pixels.Count; i++)
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
