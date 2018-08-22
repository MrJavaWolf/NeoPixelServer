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
    public class Rainbow2DEffect : INeoPixelEffect
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Description("The name of the effect")]
        public string Name { get; set; } = nameof(PulseEffect);

        [DisplayName("Enable")]
        [Description("Enable/Disable the effect.")]
        public bool IsEnabled { get; set; } = true;

        [Description("How bright the effect is (0 = off, 1 = full brightness).")]
        public float Intensity { get; set; } = 1;

        [Description("How fast the effect is.")]
        public float Speed { get; set; } = 100f;

        [Description("How zoomed in the effect is.")]
        public float Zoom { get; set; } = 3f;

        private readonly NeoStripXYCoordinates coordinates;
        private readonly NeoPixelSetup neoPixelSetup;
        private readonly RainbowColorProvider colorProvider = new RainbowColorProvider(1);
        private readonly EffectTime effectTime = new EffectTime();
        private float offset = 0;

        public Rainbow2DEffect(
            NeoPixelSetup neoPixelSetup,
            NeoStripXYCoordinates coordinates)
        {
            this.neoPixelSetup = neoPixelSetup;
            this.coordinates = coordinates;
        }

        public void Enter(EffectTime time) { }
        public void Exit(EffectTime time) { }

        public void Update(EffectTime time)
        {

            foreach (var driver in neoPixelSetup.Drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    for (int i = 0; i < strip.Pixels.Length; i++)
                    {
                        var position = coordinates.GetPosition(strip, i);
                        effectTime.DeltaTime = (long)(position.X * Zoom + offset);
                        colorProvider.Reset();
                        colorProvider.Update(effectTime);
                        Color color = colorProvider.GetColor(time);
                        Color c = Color.FromArgb(
                            (byte)(color.R * Intensity),
                            (byte)(color.G * Intensity),
                            (byte)(color.B * Intensity));
                        strip.Pixels[i] = strip.Pixels[i].Add(c);
                    }
                }
            }
            offset += Speed * time.DeltaTime / 1000.0f;
        }


    }
}
