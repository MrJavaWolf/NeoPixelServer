using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NeoPixelController.Logic
{
    public class NeoStripXYCoordinates
    {
        private readonly Dictionary<NeoPixelStrip, Vector2[]> precalculatedPixelPositions;
        public NeoStripXYCoordinates(NeoPixelSetup neoPixelSetup)
        {
            precalculatedPixelPositions = new Dictionary<NeoPixelStrip, Vector2[]>();
            foreach (var driver in neoPixelSetup.Drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    Vector2[] pixelPositions = new Vector2[strip.Pixels.Length];
                    for (int i = 0; i < strip.Pixels.Length; i++)
                    {
                        pixelPositions[i] = CalculatePosition(neoPixelSetup, strip, i + 1);
                    }
                    precalculatedPixelPositions.Add(strip, pixelPositions);
                }
            }
        }

        private Vector2 CalculatePosition(NeoPixelSetup neoPixelSetup, NeoPixelStrip strip, int pixelNumber)
        {
            if(strip.StripPosition == StripPosition.Bottom)
            {
                if (strip.RowIndex % 2 == 0)
                    return CalculateBottomLeftPosition(neoPixelSetup, strip, pixelNumber);
                else
                    return CalculateBottomRightPosition(neoPixelSetup, strip, pixelNumber);
            }
            else
            {
                if (strip.RowIndex % 2 == 0)
                    return CalculateTopLeftPosition(neoPixelSetup, strip, pixelNumber);
                else
                    return CalculateTopRightPosition(neoPixelSetup, strip, pixelNumber);
            }                            
        }

        private Vector2 CalculateBottomLeftPosition(NeoPixelSetup neoPixelSetup, NeoPixelStrip strip, int pixelNumber)
        {
            float distanceBetweenPixels = neoPixelSetup.StripLength / strip.Pixels.Length;
            double triangleSide = CalculateIsoscelesRightTriangleSide(distanceBetweenPixels);
            float y = (float)((strip.Pixels.Length - pixelNumber) * triangleSide);
            float x = (float)((strip.Pixels.Length - pixelNumber) * triangleSide + strip.RowIndex * neoPixelSetup.DistanceBetweenStrips);
            return new Vector2(x, y);
        }

        private Vector2 CalculateBottomRightPosition(NeoPixelSetup neoPixelSetup, NeoPixelStrip strip, int pixelNumber)
        {
            float distanceBetweenPixels = neoPixelSetup.StripLength / strip.Pixels.Length;
            double triangleSide = CalculateIsoscelesRightTriangleSide(distanceBetweenPixels);
            float y = (float)((strip.Pixels.Length - pixelNumber) * triangleSide);
            float x = (float)(pixelNumber * triangleSide + strip.RowIndex * neoPixelSetup.DistanceBetweenStrips);
            return new Vector2(x, y);
        }

        private Vector2 CalculateTopLeftPosition(NeoPixelSetup neoPixelSetup, NeoPixelStrip strip, int pixelNumber)
        {
            float distanceBetweenPixels = neoPixelSetup.StripLength / strip.Pixels.Length;
            double triangleSide = CalculateIsoscelesRightTriangleSide(distanceBetweenPixels);
            float y = (float)(pixelNumber * triangleSide + strip.Pixels.Length * triangleSide);
            float x = (float)((strip.Pixels.Length - pixelNumber) * triangleSide + strip.RowIndex * neoPixelSetup.DistanceBetweenStrips);
            return new Vector2(x, y);
        }

        private Vector2 CalculateTopRightPosition(NeoPixelSetup neoPixelSetup, NeoPixelStrip strip, int pixelNumber)
        {
            float distanceBetweenPixels = neoPixelSetup.StripLength / strip.Pixels.Length;
            double triangleSide = CalculateIsoscelesRightTriangleSide(distanceBetweenPixels);
            float y = (float)(pixelNumber * triangleSide + strip.Pixels.Length * triangleSide);
            float x = (float)(pixelNumber * triangleSide + strip.RowIndex * neoPixelSetup.DistanceBetweenStrips);
            return new Vector2(x, y);
        }

        private double CalculateIsoscelesRightTriangleSide(float hypotenuse)
        {
            //side^2 + side^2 = hypotenuse^2
            //2*side^2 = hypotenuse^2
            //side^2 = (hypotenuse^2) / 2
            //side = sqrt((hypotenuse^2) / 2)
            return Math.Sqrt(Math.Pow(hypotenuse, 2) / 2);
        }

        /// <summary>
        /// Get the XY position of a given pixel.
        /// (0,0) is bottom left
        /// Expects the angle to be 45 degrees between pixels
        /// </summary>
        public Vector2 GetPosition(NeoPixelStrip strip, int pixelIndex)
        {
            return precalculatedPixelPositions[strip][pixelIndex];
        }


    }
}
