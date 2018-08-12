using Microsoft.Extensions.Hosting;
using NeoPixelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NeoPixelServer.Logic
{
    public class PixelHostedService : IHostedService
    {
        private readonly PixelController pixelController;

        public PixelHostedService(PixelController pixelController)
        {
            this.pixelController = pixelController;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
           Console.WriteLine("Starting PixelController...");
            new Thread(async () =>
            {
                try
                {
                    await pixelController.StartAsync(cancellationToken);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error in PixelController:");
                    Console.WriteLine(e.ToString());
                    throw;
                }
                
            }).Start();
           

            Console.WriteLine("PixelController started");
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping PixelController...");
            await this.pixelController.StopAsync(cancellationToken);
        }
    }
}
