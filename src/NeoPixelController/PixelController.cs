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
        private NeoStripXYCoordinates coordinates;
        private readonly NeoPixelSetup neoPixelSetup;

        private readonly string[] Devices = new string[] {
            "TTYXKIYOFFPQAOFX" ,
            "YGZWFVBZJZHOSYVF",
            "WULITTLBBXPYYYVJ",
            "JJOBGWFBHVDSOMVZ"};

        public PixelController(EffectController effectController)
        {
            neoPixelSender = new NeoPixelSender("192.168.0.101", 80);
            this.effectController = effectController;

            neoPixelSetup = NeoPixelFactory.CreateNeoPixelSetup(Devices, 8, 45);
            coordinates = new NeoStripXYCoordinates(neoPixelSetup);
            effectFactory = new EffectFactory(neoPixelSetup, coordinates);

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

            effectController.AddEffect(new FullColorEffect(neoPixelSetup, new RainbowColorProvider(0.2f))
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
                isEnabled: false,
                speed: 100));

            effectController.AddEffect(effectFactory.CreateRainbow2DEffect(
                name: "2D Rainbow",
                isEnabled: true,
                zoom: 3f,
                speed: 100f));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IsRunning = true;
            Startup();
            bool wasPreviousBlack = false;
            while (IsRunning)
            {
                ResetColor(neoPixelSetup);
                var time = await timeController.UpdateTime();
                effectController.RunEffect(time);
                if (!IsBlack(neoPixelSetup) || !wasPreviousBlack)
                {
                    neoPixelSender.Send(neoPixelSetup);
                }
                wasPreviousBlack = IsBlack(neoPixelSetup);
            }
            await Task.FromResult(0);
        }

        private void ResetColor(NeoPixelSetup neoPixelSetup)
        {
            foreach (var driver in neoPixelSetup.Drivers)
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

        private bool IsBlack(NeoPixelSetup neoPixelSetup)
        {
            foreach (var driver in neoPixelSetup.Drivers)
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
    }
}
