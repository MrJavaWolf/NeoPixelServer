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
        private readonly CubicSpline interpolator;
        private readonly IColorProvider colorProvider;
        private readonly int pixelStartPosition;
        private readonly int numberOfPixels;

        //Pixels per second
        private readonly float effectSpeed = 25;
        private float offset = 0;

        public CurveEffect(
            NeoPixelStrip strip,
            IColorProvider colorProvider,
            int pixelStartPosition,
            int numberOfPixels,
            float speed)
        {
            this.strip = strip;
            this.colorProvider = colorProvider;
            this.numberOfPixels = numberOfPixels;
            this.pixelStartPosition = pixelStartPosition;
            this.effectSpeed = speed;

            Curve curve = new Curve();
            curve.AddPoint(0, 0, 0);
            curve.AddPoint(0.4, 0, 0);
            curve.AddPoint(0.5, 1, 0);
            curve.AddPoint(0.6, 0.3, 0);
            curve.AddPoint(0.8, 0.25, 0);
            curve.AddPoint(1, 0, 0);

            CubicSpline interpolator = CubicSpline.InterpolateHermite(curve.X.ToArray(), curve.Y.ToArray(), curve.W.ToArray());
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
                interpolator, 
                strip, 
                colorProvider.GetColor(time), 
                offset, 
                0, 
                20, 
                30);
            offset += effectSpeed * time.DeltaTime / 1000.0f;
        }
    }
}
