using NeoPixelController.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeoPixelServer.ViewModels
{
    public class FullColorEffectViewModel : BaseViewModel
    {
        public BaseColorProviderViewModel ColorProvider { get; set; }
    }
}
