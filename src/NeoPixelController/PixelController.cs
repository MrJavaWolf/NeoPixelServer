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
        private TimeController timeController = new TimeController();
        private EffectFactory effectFactory;
        private ResourceLoader resourceLoader = new ResourceLoader();
        private readonly IEnumerable<NeoPixelDriver> drivers;

        private readonly string[] Devices = new string[] {
            "TTYXKIYOFFPQAOFX" ,
            "YGZWFVBZJZHOSYVF",
            "WULITTLBBXPYYYVJ",
            "JJOBGWFBHVDSOMVZ"};

        public PixelController(EffectController effectController)
        {
            neoPixelSender = new NeoPixelSender("192.168.0.101", 80);
            this.effectController = effectController;

            drivers = CreateNeoPixelDrivers(Devices, 8, 45);
            effectFactory = new EffectFactory(drivers);

            effectController.AddEffect(effectFactory.CreateDefaultCurveEffect(
                name: "Rainbow",
                isEnabled: false,
                colorProvider: new RainbowColorProvider(0.2f),
                areaStartPosition: 0,
                areaLength: 45,
                effectLength: 10,
                speed: 5
                ));

            effectController.AddEffect(effectFactory.CreateDefaultCurveEffect(
               name: "Small slow rainbow",
               isEnabled: false,
               colorProvider: new RainbowColorProvider(0.2f),
               areaStartPosition: 30,
               areaLength: 15,
               effectLength: 5,
               speed: 1
               ));


            effectController.AddEffect(effectFactory.CreateDefaultCurveEffect(
               name: "Green",
               isEnabled: false,
               colorProvider: new StaticColorProvider(Color.Green),
               areaStartPosition: 0,
               areaLength: 45,
               effectLength: 13,
               speed: 3
               ));


            effectController.AddEffect(effectFactory.CreateDefaultCurveEffect(
                name: "Red",
                isEnabled: false,
                colorProvider: new StaticColorProvider(Color.Red),
                areaStartPosition: 0,
                areaLength: 45,
                effectLength: 10,
                speed: 7
                ));

            effectController.AddEffect(effectFactory.CreateDefaultCurveEffect(
                name: "Blue",
                isEnabled: false,
                colorProvider: new StaticColorProvider(Color.Blue),
                areaStartPosition: 0,
                areaLength: 45,
                effectLength: 7,
                speed: 5
                ));

            effectController.AddEffect(new FullColorEffect(drivers, new RainbowColorProvider(0.2f))
            {
                Name = "Full white",
                IsEnabled = false
            });

            //effectController.AddEffect(effectFactory.CreateScrollEffectFromTestImage(
            //    name: "Test image",
            //    isEnabled: false,
            //    speed: 10));

            effectController.AddEffect(effectFactory.CreateScrollEffectFromFile(
                name: "Color Wheel From Image",
                isEnabled: false,
                horizontalDirection: false,
                file: resourceLoader.CreateFullFilePath("colorwheel_line.png"),
                speed: 500));

            effectController.AddEffect(effectFactory.CreateColorWheelEffect(
                name: "Color Wheel",
                isEnabled: true,
                speed: 10));

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IsRunning = true;
            Startup();
            bool wasPreviousBlack = false;
            while (IsRunning)
            {
                ResetColor(drivers);
                var time = await timeController.UpdateTime();
                effectController.RunEffect(time);
                if (!IsBlack(drivers) || !wasPreviousBlack)
                {
                    neoPixelSender.Send(drivers);
                }
                wasPreviousBlack = IsBlack(drivers);
            }
            await Task.FromResult(0);
        }

        private void ResetColor(IEnumerable<NeoPixelDriver> drivers)
        {
            foreach (var driver in drivers)
            {
                foreach (var strip in driver.Strips)
                {
                    for (int i = 0; i < strip.Pixels.Length; i++)
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
            neoPixelSender.Connect();
            string rawSettings = resourceLoader.ReadSettings();
            foreach (var device in Devices)
            {
                string settings = rawSettings.Replace("<INSERT_SERIAL_HERE>", device);
                neoPixelSender.SendSettings(settings);
            }
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
                    var strip = new NeoPixelStrip()
                    {
                        Pixels = new Color[pixelsPerStrip]
                    };
                    for (int j = 0; j < pixelsPerStrip; j++)
                    {
                        strip.Pixels[j] = Color.Black;
                    }
                    driver.Strips.Add(strip);
                }
                drivers.Add(driver);
            }
            return drivers;
        }
    }
}
