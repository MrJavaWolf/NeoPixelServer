using MathNet.Numerics.Interpolation;
using NeoPixelController.Interface;
using NeoPixelController.Model;
using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic.Effects
{
    public class CurveEffect : INeoPixelEffect
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Description("The name of the effect")]
        public string Name { get; set; } = nameof(CurveEffect);

        [DisplayName("Enable")]
        [Description("Enable/Disable the effect.")]
        public bool IsEnabled { get; set; } = true;

        [Description("How bright the effect is (0 = off, 1 = full brightness).")]
        public float Intensity { get; set; } = 1;

        [DisplayName("Area Start Position")]
        [Description("How far in the effect should start.")]
        public int AreaStartPosition { get; set; } = 0;

        [DisplayName("Area Length")]
        [Description("How long the effect area should be.")]
        public int AreaLength { get; set; } = 45;

        [DisplayName("Effect Length")]
        [Description("How long the effect itself should be.")]
        public int EffectLength { get; set; } = 10;

        [Description("How fast the effect is.")]
        public float Speed { get; set; } = 2;

        public IInterpolation Interpolator { get; set; }
        public IColorProvider ColorProvider { get; set; }

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
                    Span<Color> effectArea = strip.Pixels.AsSpan(AreaStartPosition, AreaLength);
                    InterpolationEffect.Apply(
                        Interpolator,
                        effectArea,
                        ColorProvider.GetColor(time),
                        offset,
                        Intensity,
                        EffectLength);
                }
            }

            ColorProvider.Update(time);
            offset += Speed * time.DeltaTime / 1000.0f;
        }
    }
}
