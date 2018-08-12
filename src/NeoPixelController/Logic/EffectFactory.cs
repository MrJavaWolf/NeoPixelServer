using ImageMagick;
using MathNet.Numerics.Interpolation;
using NeoPixelController.Interface;
using NeoPixelController.Logic.Effects;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoPixelController.Logic
{
    public class EffectFactory
    {
        private IEnumerable<NeoPixelDriver> drivers;

        public EffectFactory(IEnumerable<NeoPixelDriver> drivers)
        {
            this.drivers = drivers;
        }

        public CurveEffect CreateDefaultCurveEffect(
            string name,
            bool isEnabled,
            IColorProvider colorProvider,
            int areaStartPosition,
            int areaLength,
            int effectLength,
            float speed,
            float intensity = 1)
        {

            Curve curve = new Curve();
            curve.AddPoint(0, 0, 0);
            curve.AddPoint(0.2, 1, 0);
            curve.AddPoint(1, 0, 0);
            var interpolator = CubicSpline.InterpolateHermite(curve.X.ToArray(), curve.Y.ToArray(), curve.W.ToArray());

            return new CurveEffect(drivers, colorProvider, interpolator)
            {
                Name = name,
                IsEnabled = isEnabled,
                AreaStartPosition = areaStartPosition,
                AreaLength = areaLength,
                EffectLength = effectLength,
                EffectSpeed = speed,
                Intensity = intensity
            };
        }


        public ScrollImageEffect CreateScrollEffectFromFile(
            string name,
            bool isEnabled,
            string file,
            float speed,
            bool horizontalDirection = true,
            float intensity = 1)
        {
            using (MagickImage image = new MagickImage(file))
            {
                return CreateScrollEffect(name, isEnabled, speed, image, horizontalDirection, intensity);
            }
        }


        public ScrollImageEffect CreateScrollEffectFromTestImage(
            string name,
            bool isEnabled,
            float speed,
            bool horizontalDirection = true,
            float intensity = 1)
        {
            using (MagickImage image = new MagickImage(MagickColor.FromRgb(255, 0, 0), 200, 100))
            {
                var draw = new Drawables();
                draw.FillColor(MagickColor.FromRgb(0, 0, 255));
                draw.BorderColor(MagickColor.FromRgb(0, 0, 0));
                image.Draw(draw.Ellipse(30, 30, 20, 20, 0, 360));
                image.Draw(draw.Ellipse(75, 75, 20, 20, 0, 360));
                image.Draw(draw.Ellipse(150, 50, 30, 30, 0, 360));
                //image.Write("image.png");
                return CreateScrollEffect(name, isEnabled, speed, image, horizontalDirection, intensity);
            }
        }

        public ScrollImageEffect CreateScrollEffect(
            string name,
            bool isEnabled,
            float speed,
            MagickImage image,
            bool horizontalDirection = true,
            float intensity = 1)
        {
            return new ScrollImageEffect(drivers,
                image.GetPixels().GetValues(),
                image.Width,
                image.Height,
                image.ChannelCount)
                {
                    Speed = speed,
                    Name = name,
                    IsEnabled = isEnabled,
                    Intensity = intensity,
                    Horizontal = horizontalDirection
            };

        }
    }
}
