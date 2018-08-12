using ImageMagick;
using NeoPixelController.Interface;
using NeoPixelController.Logic.Extension;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic.Effects
{
    public class WaveEffect : INeoPixelEffect
    {

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = nameof(WaveEffect);
        public bool IsEnabled { get; set; } = true;
        public float Intensity { get; set; } = 1f;
        public float Speed { get; set; } = 50;

        private readonly IEnumerable<NeoPixelDriver> drivers;

        private float offset = 0;
        private readonly byte[] pixels;
        private readonly int width = 200;
        private readonly int height = 100;

        public WaveEffect(IEnumerable<NeoPixelDriver> drivers)
        {
            this.drivers = drivers;
            using (MagickImage image = new MagickImage(MagickColor.FromRgb(255, 0, 0), width, height))
            {
                var draw = new Drawables();
                draw.FillColor(MagickColor.FromRgb(0, 0, 255));
                draw.BorderColor(MagickColor.FromRgb(0, 0, 0));
                image.Draw(draw.Ellipse(30, 30, 20, 20, 0, 360));
                image.Draw(draw.Ellipse(75, 75, 20, 20, 0, 360));
                image.Draw(draw.Ellipse(150, 50, 30, 30, 0, 360));
                //image.Write("image.png");
                pixels = image.GetPixels().GetValues();
            }
        }


        public void Enter(EffectTime time)
        {
        }

        public void Exit(EffectTime time)
        {
        }


        public void Update(EffectTime time)
        {
            int xOffset = (int)(offset % width);
            foreach (var driver in drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    var yStep = height / strip.Pixels.Count;
                    for (int i = 0; i < strip.Pixels.Count; i++)
                    {
                        int pixelIndex = width * 4 * yStep * i + xOffset * 4;
                        strip.Pixels[i] = strip.Pixels[i].Add(Color.FromArgb(
                            (byte)(pixels[pixelIndex + 0] * Intensity),
                            (byte)(pixels[pixelIndex + 1] * Intensity),
                            (byte)(pixels[pixelIndex + 2] * Intensity)));
                    }
                }
            }

            offset += Speed * time.DeltaTime / 1000.0f;
        }


    }
}
