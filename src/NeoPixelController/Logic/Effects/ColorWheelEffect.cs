using NeoPixelController.Interface;
using NeoPixelController.Logic.ColorProviders;
using NeoPixelController.Logic.Extension;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic.Effects
{
    public class ColorWheelEffect : INeoPixelEffect
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Description("The name of the effect")]
        public string Name { get; set; } = nameof(ColorWheelEffect);

        [DisplayName("Enable")]
        [Description("Enable/Disable the effect.")]
        public bool IsEnabled { get; set; } = true;

        [Description("How bright the effect is (0 = off, 1 = full brightness).")]
        public float Intensity { get; set; } = 1;

        [Description("How fast the effect is.")]
        public float Speed { get; set; } = 2;



        private IEnumerable<NeoPixelDriver> drivers;
        private float offset = 0;
        private EffectTime effectTime = new EffectTime();
        private RainbowColorProvider colorProvider;

        public ColorWheelEffect(
            IEnumerable<NeoPixelDriver> drivers)
        {
            this.drivers = drivers;
            colorProvider = new RainbowColorProvider(1);
        }

        public void Enter(EffectTime time)
        {

        }

        public void Exit(EffectTime time)
        {

        }

        public void Update(EffectTime time)
        {

            foreach (var driver in drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    float timeStep = 1.0f / strip.Pixels.Length;
                    effectTime.DeltaTime = (long)(timeStep * 1000);
                    for (int i = 0; i < strip.Pixels.Length; i++)
                    {
                        Color c = colorProvider.GetColor(effectTime);
                        strip.Pixels[i].Add(Color.FromArgb(
                            (byte)(c.R * Intensity),
                            (byte)(c.G * Intensity),
                            (byte)(c.B * Intensity)));
                        colorProvider.Update(effectTime);
                    }
                }
            }

            colorProvider.Update(time);
            offset += Speed * time.DeltaTime / 1000.0f;
        }
    }
}
