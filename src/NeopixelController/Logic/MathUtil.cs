using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeopixelController.Logic
{
    public class MathUtil
    {
        public static double Clamp(double min, double max, double value)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}
