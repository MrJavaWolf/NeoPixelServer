using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoPixelController.Interface
{
    public interface INeoPixelEffect
    {
        void Enter(EffectTime time);
        void Update(EffectTime time);
        void Exit(EffectTime time);
    }
}
