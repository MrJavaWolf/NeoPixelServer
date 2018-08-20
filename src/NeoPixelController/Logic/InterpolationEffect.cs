using MathNet.Numerics.Interpolation;
using NeoPixelController.Logic;
using NeoPixelController.Logic.Extension;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoPixelController.Logic
{
    public class InterpolationEffect
    {
        public static void Apply(
            IInterpolation interpolator,
            Span<Color> toPixels,
            Color color,
            float time,
            float intensity,
            int effectLength)
        {
            var start = Math.Abs(time) % toPixels.Length;
            int startPixel = (int)start;
            float tStep = 1 / (float)effectLength;
            float timeOffset = tStep * (1 - Math.Abs(start - (int)start));
            for (int i = 0; i < effectLength; i++)
            {
                float t = 1 - (tStep * i + timeOffset);
                var interpolation = interpolator.Interpolate(t);
                float colorIntensity = (float)MathUtil.Clamp(0, 1, interpolation * intensity);

                int index = (startPixel + i) % toPixels.Length;
                if (index < toPixels.Length)
                    toPixels[index] =
                        toPixels[index].Add(
                            Color.FromArgb(
                                (int)(color.R * colorIntensity),
                                (int)(color.G * colorIntensity),
                                (int)(color.B * colorIntensity)));
            }
        }
    }
}
