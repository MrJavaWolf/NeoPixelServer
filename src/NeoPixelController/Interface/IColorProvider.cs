using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Interface
{
    public interface IColorProvider
    {
        Guid Id { get; }
        string Name { get; set; }
        Color GetColor(EffectTime time);
    }
}
