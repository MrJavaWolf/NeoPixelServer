using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoPixelController.Model
{
    public class NeoPixelStrip
    {
        public List<Color> Pixels { get; set; } = new List<Color>();
    }
}
