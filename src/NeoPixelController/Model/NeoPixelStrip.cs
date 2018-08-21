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
        public StripPosition StripPosition { get; set; }
        public int RowIndex { get; set; }
        public Color[] Pixels { get; set; }
    }
}
