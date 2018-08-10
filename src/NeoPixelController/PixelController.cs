using MathNet.Numerics.Interpolation;
using NeoPixelController.Logic;
using NeoPixelController.Logic.ColorProviders;
using NeoPixelController.Logic.Effects;
using NeoPixelController.Model;
using System;
using System.Collections.Generic;
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
            neoPixelSender = new NeoPixelSender("ws://192.168.0.101");
            this.effectController = effectController;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IsRunning = true;
            Startup();
            IEnumerable<NeoPixelDriver> drivers = CreateNeoPixelDrivers(new string[] { "TTYXKIYOFFPQAOFX" }, 7, 45);
            effectController.AddEffect(new CurveEffect(drivers, new RainbowColorProvider(0.2f), 0, 45, 10, 5)
            {
                Name = "Rainbow"
            });
            effectController.AddEffect(new CurveEffect(drivers, new RainbowColorProvider(0.2f), 30, 45, 5, 1)
            {
                Name = "Small slow rainbow",
                IsEnabled = false
            });
            effectController.AddEffect(new CurveEffect(drivers, new StaticColorProvider(Color.Green), 0, 45, 13, 3)
            {
                Name = "Green",
                IsEnabled = false
            });
            effectController.AddEffect(new CurveEffect(drivers, new StaticColorProvider(Color.Red), 0, 45, 10, 7)
            {
                Name = "Red",
                IsEnabled = false
            });
            effectController.AddEffect(new CurveEffect(drivers, new StaticColorProvider(Color.Blue), 0, 45, 7, 5)
            {
                Name = "Blue",
                IsEnabled = false
            });
            bool wasPreviousBlack = false;
            while (IsRunning)
            {
                ResetColor(drivers);
                effectController.RunEffect();
                if (!IsBlack(drivers) || !wasPreviousBlack)
                {
                    neoPixelSender.Send(drivers);
                }
                wasPreviousBlack = IsBlack(drivers);

                await Task.Delay(15);
            }
            await Task.FromResult(0);
        }

        private void ResetColor(IEnumerable<NeoPixelDriver> drivers)
        {
            foreach (var driver in drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    for (int i = 0; i < strip.Pixels.Count; i++)
                    {
                        strip.Pixels[i] = Color.Black;
                    }
                }
            }
        }

        private bool IsBlack(IEnumerable<NeoPixelDriver> drivers)
        {
            foreach (var driver in drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    foreach (var pixel in strip.Pixels)
                    {
                        if (pixel != Color.Black)
                            return false;
                    }
                }
            }
            return true;
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

        private IEnumerable<NeoPixelDriver> CreateNeoPixelDrivers(
            IEnumerable<string> driverNames,
            int stripsPerDriver,
            int pixelsPerStrip)
        {
            List<NeoPixelDriver> drivers = new List<NeoPixelDriver>();
            foreach (var driverName in driverNames)
            {
                var driver = new NeoPixelDriver()
                {
                    Name = driverName
                };
                for (int i = 0; i < stripsPerDriver; i++)
                {
                    var strip = new NeoPixelStrip();
                    for (int j = 0; j < pixelsPerStrip; j++)
                    {
                        strip.Pixels.Add(Color.Black);
                    }
                    driver.Strips.Add(strip);
                }
                drivers.Add(driver);
            }
            return drivers;
        }
    }
}
