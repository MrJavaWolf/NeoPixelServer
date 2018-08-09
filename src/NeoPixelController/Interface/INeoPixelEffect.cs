using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoPixelController.Interface
{
    public interface INeoPixelEffect
    {
        Guid Id { get; }
        string Name { get; set; }
        bool IsEnabled { get; set; }

        void Enter(EffectTime time);
        void Update(EffectTime time);
        void Exit(EffectTime time);
    }
}
