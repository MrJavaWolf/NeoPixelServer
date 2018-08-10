using System;
using System.Collections.Generic;
using System.Text;

namespace NeoPixelController.Model
{
    public class NeoPixelDriver
    {
        public string Name { get; set; }
        public List<NeoPixelStrip> Strips { get; set; } = new List<NeoPixelStrip>();
    }
}
