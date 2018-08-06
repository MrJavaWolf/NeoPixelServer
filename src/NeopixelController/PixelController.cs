using MathNet.Numerics.Interpolation;
using NeoPixelController.Logic;
using NeoPixelController.Logic.ColorProviders;
using NeoPixelController.Logic.Effects;
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
        public bool IsRunning { get; private set; }
        private NeoPixelSender neoPixelSender;
        private EffectController effectController;

        public PixelController(EffectController effectController)
        {
            neoPixelSender = new NeoPixelSender("ws://192.168.0.12");
            this.effectController = effectController;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IsRunning = true;
            Startup();
            var strip = CreateStrip();
            effectController.AddEffect(new CurveEffect(strip, new RainbowColorProvider(0.2f), 0, 64, 15, 5));
            effectController.AddEffect(new CurveEffect(strip, new StaticColorProvider(Color.Green), 50, 14, 10, -2));
            //effectController.AddEffect(new CurveEffect(strip, new RainbowColorProvider(0.1f), 0, 64, -10));
            //effectController.AddEffect(new FullColorEffect(strip, new RainbowColorProvider(0.1f)));
            while (IsRunning)
            {
                for (int i = 0; i < strip.Pixels.Count; i++)
                {
                    strip.Pixels[i] = Color.Black;
                }
                effectController.RunEffect();
                neoPixelSender.Send(strip);
                Thread.Sleep(15);
            }
            await Task.FromResult(0);
        }

        private void Startup()
        {
            //get the full location of the assembly with DaoTests in it
            string fullPath = Assembly.GetExecutingAssembly().Location;

            //get the folder that's in
            string theDirectory = Path.GetDirectoryName(fullPath);
            neoPixelSender.Connect();
            neoPixelSender.SendSettings(File.ReadAllText(Path.Combine(theDirectory, "Json", "SetDeviceOptions.json")));
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

            for (int i = 0; i < 64; i++)
            {
                strip.Pixels.Add(Color.Black);
            }
            return strip;
        }
    }
}
