using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeoPixelServer.ViewModels
{
    public class ScrollImageEffectViewModel : BaseViewModel
    {
        public float Speed { get; set; }
        public bool Horizontal { get; set; }
    }
}
