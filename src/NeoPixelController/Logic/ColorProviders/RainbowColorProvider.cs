using NeoPixelController.Interface;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NeoPixelController.Logic.ColorProviders
{
    public class RainbowColorProvider : IColorProvider
    {
        private readonly float speed;
        private float offset = 0;

        public RainbowColorProvider(float speed)
        {
            this.speed = speed;
        }

        public Color GetColor(EffectTime time)
        {
            offset += speed * time.DeltaTime / 1000.0f;

            //https://stackoverflow.com/questions/2288498/how-do-i-get-a-rainbow-color-gradient-in-c
            float div = (Math.Abs(offset % 1) * 6);
            int ascending = (int)((div % 1) * 255);
            int descending = 255 - ascending;
            switch ((int)div)
            {
                case 0:
                    return Color.FromArgb(255, 255, ascending, 0);
                case 1:
                    return Color.FromArgb(255, descending, 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, ascending);
                case 3:
                    return Color.FromArgb(255, 0, descending, 255);
                case 4:
                    return Color.FromArgb(255, ascending, 0, 255);
                default: // case 5:
                    return Color.FromArgb(255, 255, 0, descending);
            }
        }
    }
}
