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
        private readonly int effectLength;

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
            this.colorProvider = colorProvider;
            this.numberOfPixels = numberOfPixels;
            this.effectLength = effectLength;
            this.pixelStartPosition = pixelStartPosition;
            this.effectSpeed = speed;

            Curve curve = new Curve();
            curve.AddPoint(0, 0, 0);
            curve.AddPoint(0.1, 1, 0);
            curve.AddPoint(0.2, 0.35, 0);
            curve.AddPoint(0.9, 0.25, 0);
            curve.AddPoint(1, 0, 0);

            this.interpolator = CubicSpline.InterpolateHermite(curve.X.ToArray(), curve.Y.ToArray(), curve.W.ToArray());
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
                pixelStartPosition,
                numberOfPixels,
                effectLength);
            offset += effectSpeed * time.DeltaTime / 1000.0f;
        }
    }
}
