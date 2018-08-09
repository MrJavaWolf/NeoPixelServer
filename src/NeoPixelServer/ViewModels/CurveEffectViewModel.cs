using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeoPixelServer.ViewModels
{
    public class CurveEffectViewModel : BaseViewModel
    {
        public int PixelStartPosition { get; set; }
        public int NumberOfPixels { get; set; }
        public int EffectLength { get; set; }
        public float EffectSpeed { get; set; }
        public BaseColorProviderViewModel ColorProvider { get; set; }
        
    }
}
