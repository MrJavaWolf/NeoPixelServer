using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeoPixelServer.ViewModels
{
    public class PulseEffectViewModel : BaseViewModel
    {
        public BaseColorProviderViewModel ColorProvider { get; set; }
        public int AreaStartPosition { get; set; }
        public int AreaLength { get; set; }
        public float EffectSpeed { get; set; }
    }
}
