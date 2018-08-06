using MathNet.Numerics.Interpolation;
using NeoPixelController.Interface;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic.Effects
{
    public class CurveEffect : INeoPixelEffect
    {
        private readonly NeoPixelStrip strip;
        public CubicSpline Interpolator { get; set; }
        public IColorProvider ColorProvider { get; set; }
        public int PixelStartPosition { get; set; }
        public int NumberOfPixels { get; set; }
        public int EffectLength { get; set; }

        //Pixels per second
        private readonly float effectSpeed = 25;
        private float offset = 0;

        public CurveEffect(
            NeoPixelStrip strip,
            IColorProvider colorProvider,
            int pixelStartPosition,
            int numberOfPixels,
            int effectLength,
            float speed)
        {
            this.strip = strip;
            this.ColorProvider = colorProvider;
            this.NumberOfPixels = numberOfPixels;
            this.EffectLength = effectLength;
            this.PixelStartPosition = pixelStartPosition;
            this.effectSpeed = speed;

            Curve curve = new Curve();
            curve.AddPoint(0, 0, 0);
            curve.AddPoint(0.1, 1, 0);
            curve.AddPoint(0.2, 0.35, 0);
            curve.AddPoint(0.9, 0.25, 0);
            curve.AddPoint(1, 0, 0);

            this.Interpolator = CubicSpline.InterpolateHermite(curve.X.ToArray(), curve.Y.ToArray(), curve.W.ToArray());
        }


        public void Enter(EffectTime time)
        {

        }

        public void Exit(EffectTime time)
        {

        }

        public void Update(EffectTime time)
        {
            InterpolationEffect.Apply(
                Interpolator,
                strip,
                ColorProvider.GetColor(time),
                offset,
                0,
                PixelStartPosition,
                NumberOfPixels,
                EffectLength);
            offset += effectSpeed * time.DeltaTime / 1000.0f;
        }
    }
}
