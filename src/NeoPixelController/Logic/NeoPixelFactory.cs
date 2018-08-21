using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic
{
    public class NeoPixelFactory
    {

        public static NeoPixelSetup CreateNeoPixelSetup(
            IEnumerable<string> driverNames,
            int stripsPerDriver,
            int pixelsPerStrip)
        {
            List<NeoPixelDriver> drivers = new List<NeoPixelDriver>();
            int stripCount = 0;
            foreach (var driverName in driverNames)
            {
                var driver = new NeoPixelDriver()
                {
                    Name = driverName
                };
                for (int i = 0; i < stripsPerDriver; i++)
                {
                    var strip = new NeoPixelStrip()
                    {
                        Pixels = new Color[pixelsPerStrip],
                        StripPosition = stripCount % 2 == 0 ? StripPosition.Bottom : StripPosition.Top,
                        RowIndex = stripCount / 2
                    };
                    stripCount++;
                    for (int j = 0; j < pixelsPerStrip; j++)
                    {
                        strip.Pixels[j] = Color.Black;
                    }
                    driver.Strips.Add(strip);
                }
                drivers.Add(driver);
            }

            //Measurements from RL
            float distanceBetweenStrips = 10.666f;
            float stripLength = 75f;

            return new NeoPixelSetup()
            {
                Drivers = drivers,
                DistanceBetweenStrips = distanceBetweenStrips,
                StripLength = stripLength,
            };
        }
    }
}
