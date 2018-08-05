using MathNet.Numerics.Interpolation;
using NeopixelController.Logic;
using NeopixelController.Model;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace NeopixelController
{
    public class PixelController
    {

        private const int SkipPixels = 18;
        private const int NumberOfPixels = 38;

        public AnimationType AnimationType { get; set; } = AnimationType.HermitCurve;
        public bool IsRunning { get; private set; }

        public void CurveEffect(WebSocket ws, NeoPixelStrip strip)
        {

            Curve curve = new Curve();

            curve.AddPoint(0, 0, 0);
            curve.AddPoint(0.4, 0, 0);
            curve.AddPoint(0.5, 1, 0);
            curve.AddPoint(0.6, 0.3, 0);
            curve.AddPoint(0.8, 0.25, 0);
            curve.AddPoint(1, 0, 0);

            CubicSpline interpolator = CubicSpline.InterpolateHermite(curve.X.ToArray(), curve.Y.ToArray(), curve.W.ToArray());

            //double[] times = new double[] { 0, 0.1, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.7, 0.75, 0.8, 0.9, 1 };
            //double[] values = new double[times.Length];
            //for (int i = 0; i < times.Length; i++)
            //{
            //    values[i] = interpolator.Interpolate(times[i]);
            //    Console.WriteLine(times[i] + ";" + values[i]);
            //}

            float IntensityDamping = 0f;
            float time = 0;
            double offsetSpeed = 20f; //Pixels per second
            int sleepTimeMs = 5;
            while (AnimationType == AnimationType.HermitCurve)
            {
                for (int i = 0; i < strip.Pixels.Count; i++) strip.Pixels[i] = Color.Black;
                InterpolationEffect.Apply(interpolator, strip, Color.BlueViolet, time, IntensityDamping, 20, 30);
                InterpolationEffect.Apply(interpolator, strip, Color.Red, -time * 0.3f, IntensityDamping, 30, 20);
                //Console.WriteLine("Offset: " + offset);
                ws.Send(GetBytes(strip));
                Thread.Sleep(sleepTimeMs);
                time += (float)(offsetSpeed / 1000.0 * sleepTimeMs);
            }

        }

        private void SetColor(WebSocket ws, NeoPixelStrip strip, Color color)
        {

            for (int i = 0; i < strip.Pixels.Count; i++)
            {
                strip.Pixels[i] = color;
            }
            ws.Send(GetBytes(strip));
            ws.Send(GetBytes(strip));
            while (AnimationType == AnimationType.SingleColor)
            {
                Thread.Sleep(80);
            }
        }

        private NeoPixelStrip CreateStrip()
        {
            NeoPixelStrip strip = new NeoPixelStrip
            {
                DeviceName = "TTYXKIYOFFPQAOFX"
            };

            for (int i = 0; i < 73; i++)
            {
                strip.Pixels.Add(Color.Black);
            }
            return strip;
        }

        private void Pulse(WebSocket ws, NeoPixelStrip strip)
        {
            double offset = 0;
            int offsetSpeed = 25; //Pixels per second
            int sleepTimeMs = 15;
            float intensity = 1f;
            while (AnimationType == AnimationType.Pulse)
            {
                for (int i = SkipPixels; i < strip.Pixels.Count && i < SkipPixels + NumberOfPixels; i++)
                {
                    double rawCalculation = 255.0 / strip.Pixels.Count * i * intensity;
                    rawCalculation = (rawCalculation / 255.0) * rawCalculation;
                    byte calculated = (byte)(rawCalculation);
                    strip.Pixels[SkipPixels + (i + (int)offset) % NumberOfPixels] = Color.FromArgb(calculated, 0, 0);
                }
                ws.Send(GetBytes(strip));
                Thread.Sleep(sleepTimeMs);
                offset += offsetSpeed / 1000.0 * sleepTimeMs;
            }
        }


        public byte[] GetBytes(NeoPixelStrip strip)
        {

            byte[] bytes = new byte[1 + 1 + 2 + strip.Pixels.Count * 3];

            bytes[0] = strip.Channel;
            bytes[1] = CommandType.Set8BitPixelColours;
            //bytes[2] = ; - Do not set
            //bytes[3] = ; - Do not set
            for (int i = 0; i < strip.Pixels.Count; i++)
            {
                bytes[4 + i * 3 + 0] = strip.Pixels[i].R;
                bytes[4 + i * 3 + 1] = strip.Pixels[i].G;
                bytes[4 + i * 3 + 2] = strip.Pixels[i].B;
            }
            return bytes;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IsRunning = true;
            using (var ws = new WebSocket("ws://192.168.0.12"))
            {
                ws.OnMessage += (sender, e) =>
                    Console.WriteLine("Server: " + e.Data);
                //get the full location of the assembly with DaoTests in it
                string fullPath = Assembly.GetExecutingAssembly().Location;

                //get the folder that's in
                string theDirectory = Path.GetDirectoryName(fullPath);

                ws.Connect();
                ws.Send(File.ReadAllText(Path.Combine(theDirectory, "Json", "SetDeviceOptions.json")));
                var strip = CreateStrip();
                while (IsRunning)
                {
                    Thread.Sleep(80);
                    switch (AnimationType)
                    {
                        case AnimationType.HermitCurve:
                            CurveEffect(ws, strip);
                            break;
                        case AnimationType.SingleColor:
                            SetColor(ws, strip, Color.Blue);
                            break;
                        case AnimationType.Pulse:
                            Pulse(ws, strip);
                            break;
                    }
                }
            }

            await Task.FromResult(0);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            IsRunning = false;
            AnimationType = AnimationType.None;
            await Task.FromResult(0);
        }
    }
}
