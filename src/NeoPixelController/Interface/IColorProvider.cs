using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Interface
{
    public interface IColorProvider
    {
        Color GetColor(EffectTime time);
    }
}
