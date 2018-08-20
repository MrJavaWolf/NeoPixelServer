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
    public class ScrollImageEffect : INeoPixelEffect
    {

        public Guid Id { get; private set; } = Guid.NewGuid();

        [Description("The name of the effect")]
        public string Name { get; set; } = nameof(ScrollImageEffect);

        [DisplayName("Enable")]
        [Description("Enable/Disable the effect.")]
        public bool IsEnabled { get; set; } = true;

        [Description("How bright the effect is (0 = off, 1 = full brightness).")]
        public float Intensity { get; set; } = 1f;

        [DisplayName("Speed")]
        [Description("How fast the effect is.")]
        public float Speed { get; set; } = 50;

        [Description("Scroll horizontal (on) or vertial (off).")]
        public bool Horizontal { get; set; } = true;

        private readonly IEnumerable<NeoPixelDriver> drivers;
        private readonly Bitmap image;

        private float offset = 0;

        public ScrollImageEffect(IEnumerable<NeoPixelDriver> drivers, Bitmap image)
        {
            this.drivers = drivers;
            this.image = image;
        }


        public void Enter(EffectTime time)
        {
        }

        public void Exit(EffectTime time)
        {
        }


        public void Update(EffectTime time)
        {
            int xOffset = (int)(offset % image.Width);
            int yOffset = (int)(offset % image.Height);
            foreach (var driver in drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    var yStep = image.Height / strip.Pixels.Count;
                    for (int i = 0; i < strip.Pixels.Count; i++)
                    {
                        int x, y;
                        if (Horizontal)
                        {
                            x = xOffset;
                            y = yStep * i;
                        }
                        else
                        {
                            x = 0;
                            y = (yStep * i + yOffset) % image.Height;
                        }


                        var color = image.GetPixel(x, y);
                        strip.Pixels[i] = strip.Pixels[i].Add(Color.FromArgb(
                            (byte)(color.R * Intensity),
                            (byte)(color.G * Intensity),
                            (byte)(color.B * Intensity)));
                    }
                }
            }

            offset += Speed * time.DeltaTime / 1000.0f;
        }


    }
}
