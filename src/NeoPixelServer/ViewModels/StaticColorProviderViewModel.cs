using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace NeoPixelServer.ViewModels
{
    public class StaticColorProviderViewModel : BaseColorProviderViewModel
    {
        public Color Color { get; set; }
    }
}
