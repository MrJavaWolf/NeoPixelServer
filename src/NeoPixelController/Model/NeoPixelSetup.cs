using System;
using System.Collections.Generic;
using System.Text;

namespace NeoPixelController.Model
{
    public class NeoPixelSetup
    {
        public List<NeoPixelDriver> Drivers { get; set; } = new List<NeoPixelDriver>();

        /// <summary>
        /// The distance between the strips in cm
        /// </summary>
        public float DistanceBetweenStrips { get; set; } = 10.666f;

        /// <summary>
        /// The length of the strips in cm
        /// </summary>
        public float StripLength { get; set; } = 75f;

    }
}
