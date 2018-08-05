using MathNet.Numerics.Interpolation;
using NeoPixelController.Logic;
using NeoPixelController.Model;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace NeoPixelController
{
    public class PixelController
    {

        private const int SkipPixels = 18;
        private const int NumberOfPixels = 38;

        public bool IsRunning { get; private set; }

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
                  
                }
            }

            await Task.FromResult(0);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            IsRunning = false;
            await Task.FromResult(0);
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
    }
}
