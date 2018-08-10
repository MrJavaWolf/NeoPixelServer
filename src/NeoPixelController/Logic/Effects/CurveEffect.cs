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
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = nameof(CurveEffect);
        public bool IsEnabled { get; set; } = true;
        public IInterpolation Interpolator { get; set; }
        public IColorProvider ColorProvider { get; set; }
        public int AreaStartPosition { get; set; }
        public int AreaLength { get; set; }
        public int EffectLength { get; set; }
        public float EffectSpeed { get; set; }
        public float Intensity { get; set; } = 1;

        private readonly NeoPixelStrip strip;
        private float offset = 0;

        public CurveEffect(
            NeoPixelStrip strip,
            IColorProvider colorProvider,
            int AreaStartPosition,
            int AreaLength,
            int effectLength,
            float speed)
        {
            this.strip = strip;
            this.ColorProvider = colorProvider;
            this.AreaStartPosition = AreaStartPosition;
            this.AreaLength = AreaLength;
            this.EffectLength = effectLength;
            this.EffectSpeed = speed;

            Curve curve = new Curve();
            curve.AddPoint(0, 0, 0);
            curve.AddPoint(0.2, 1, 0);
            //curve.AddPoint(0.5, 0.35, 0);
            //curve.AddPoint(0.9, 0.25, 0);
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
                Intensity,
                AreaStartPosition,
                AreaLength,
                EffectLength);
            offset += EffectSpeed * time.DeltaTime / 1000.0f;
        }
    }
}
