using MathNet.Numerics.Interpolation;
using NeoPixelController.Interface;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using WebSocketSharp;

namespace NeoPixelController
{
    public class EffectController
    {
        private List<INeoPixelEffect> effects = new List<INeoPixelEffect>();

        internal void RunEffect(EffectTime time)
        {
            foreach (var effect in effects)
            {
                if (effect.IsEnabled)
                {
                    effect.Update(time);
                }
            }
        }

        public void AddEffect(INeoPixelEffect effect)
        {
            effects.Add(effect);
        }

        public void RemoveEffect(INeoPixelEffect effect)
        {
            effects.Remove(effect);
        }

        public void RemoveEffect(Guid Id)
        {
            var effect = GetEffects().Where(e => e.Id == Id).FirstOrDefault();
            if (effect != null) effects.Remove(effect);
        }


        public IReadOnlyList<INeoPixelEffect> GetEffects()
        {
            return effects;
        }

        public INeoPixelEffect GetEffect(Guid Id)
        {
            return GetEffects().Where(e => e.Id == Id).FirstOrDefault();
        }

        public T GetEffect<T>(Guid Id)
            where T : INeoPixelEffect
        {
            var effect = GetEffects().Where(e => e.Id == Id).FirstOrDefault();
            if (effect is T effectWithType)
                return effectWithType;
            else
                return default(T);
        }

    }
}
