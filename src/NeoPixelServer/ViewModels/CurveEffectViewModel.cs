using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeoPixelServer.ViewModels
{
    public class CurveEffectViewModel : BaseViewModel
    {
        public int AreaStartPosition { get; set; }
        public int AreaLength { get; set; }
        public int EffectLength { get; set; }
        public float EffectSpeed { get; set; }
        public BaseColorProviderViewModel ColorProvider { get; set; }
        
    }
}
