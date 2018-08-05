using NeopixelController.Logic;
using NeopixelController.Model;
using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeopixelController.Logic
{
    public class InterpolationEffect
    {
        public static void Apply(
            IInterpolation interpolator,
            NeoPixelStrip toStrip,
            Color color,
            float time,
            float intensityDamping,
            int pixelStartPosition,
            int numberOfPixels)
        {
            var intensityMultiplier = (1 - intensityDamping);
            for (int i = 0; i < numberOfPixels; i++)
            {
                float t = (Math.Abs(i + time) % numberOfPixels) / numberOfPixels;
                var interpolation = interpolator.Interpolate(t);
                float colorIntensity = (float)MathUtil.Clamp(0, 1, interpolation * intensityMultiplier);

                var currentColor = toStrip.Pixels[i + pixelStartPosition];
                if (i + pixelStartPosition < toStrip.Pixels.Count)
                    toStrip.Pixels[i + pixelStartPosition] = Color.FromArgb(
                        (byte)Math.Min(255, (color.R * colorIntensity + currentColor.R)),
                        (byte)Math.Min(255, color.G * colorIntensity + currentColor.G),
                        (byte)Math.Min(255, color.B * colorIntensity + currentColor.B));
            }
        }
    }
}
