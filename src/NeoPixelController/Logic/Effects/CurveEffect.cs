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
        public int AreaStartPosition { get; set; } = 0;
        public int AreaLength { get; set; } = 40;
        public int EffectLength { get; set; } = 10;
        public float EffectSpeed { get; set; } = 2;
        public float Intensity { get; set; } = 1;

        private readonly IEnumerable<NeoPixelDriver> drivers;
        private float offset = 0;

        public CurveEffect(
            IEnumerable<NeoPixelDriver> drivers,
            IColorProvider colorProvider,
            IInterpolation interpolator)
        {
            this.drivers = drivers;
            this.ColorProvider = colorProvider;
            this.Interpolator = interpolator;
        }


        public void Enter(EffectTime time)
        {

        }

        public void Exit(EffectTime time)
        {

        }

        public void Update(EffectTime time)
        {
            foreach (var driver in drivers)
            {
                foreach (var strip in driver.Strips)
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
                }
            }

            ColorProvider.Update(time);
            offset += EffectSpeed * time.DeltaTime / 1000.0f;
        }
    }
}
