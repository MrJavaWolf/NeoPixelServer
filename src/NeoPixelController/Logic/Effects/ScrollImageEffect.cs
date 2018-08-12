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
    public class ScrollImageEffect : INeoPixelEffect
    {

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = nameof(ScrollImageEffect);
        public bool IsEnabled { get; set; } = true;
        public float Intensity { get; set; } = 1f;
        public float Speed { get; set; } = 50;
        public bool Horizontal { get; set; } = true;

        private readonly IEnumerable<NeoPixelDriver> drivers;
        private readonly byte[] pixels;
        private readonly int width;
        private readonly int height;
        private readonly int channels;

        private float offset = 0;

        public ScrollImageEffect(IEnumerable<NeoPixelDriver> drivers, byte[] pixels, int width, int height, int channels)
        {
            this.drivers = drivers;
            this.pixels = pixels;
            this.width = width;
            this.height = height;
            this.channels = channels;
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
            int yOffset = (int)(offset % height);
            foreach (var driver in drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    var yStep = height / strip.Pixels.Count;
                    for (int i = 0; i < strip.Pixels.Count; i++)
                    {
                        int pixelIndex = Horizontal ?
                            width * channels * yStep * i + xOffset * channels :
                           (width * channels * yStep * i + yOffset * channels * width) % pixels.Length;

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
