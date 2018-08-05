using NeoPixelController.Logic;
using NeoPixelController.Model;
using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoPixelController.Logic.Extension;

namespace NeoPixelController.Logic
{
    public class InterpolationEffect
    {
        public static void Apply(
            IInterpolation interpolator,
            NeoPixelStrip toStrip,
            Color color,
            float time,
            float intensityDamping,
            int areaStartPosition,
            int areaLength,
            int effectLength)
        {
            var intensityMultiplier = (1 - intensityDamping);
            var start = time % areaLength;

            int startPixel = areaStartPosition + (int)start;
            int endPixel = Math.Min(startPixel + effectLength, toStrip.Pixels.Count);

            float tStep = 1 / (float)effectLength;
            float timeOffset;

            if (time > 0) //Time is going forward
                timeOffset = tStep * (1 - Math.Abs(start - (int)start));
            else //Time is going backwards
                timeOffset = tStep * (Math.Abs(start - (int)start));
            for (int i = 0; i < effectLength; i++)
            {
                float t = tStep * i + timeOffset;
                if (time > 0) //Time is going forward
                    t = 1 - t;
                var interpolation = interpolator.Interpolate(t);
                float colorIntensity = (float)MathUtil.Clamp(0, 1, interpolation * intensityMultiplier);

                int index = (startPixel + i) % areaLength;
                if (index < areaStartPosition) index += areaStartPosition;

                if (index < toStrip.Pixels.Count)
                    toStrip.Pixels[index] =
                        toStrip.Pixels[index].Add(
                            Color.FromArgb(
                                (int)(color.R * colorIntensity),
                                (int)(color.G * colorIntensity),
                                (int)(color.B * colorIntensity)));
            }
        }
    }
}
