using Microsoft.Extensions.Hosting;
using NeopixelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NeopixelServer.Logic
{
    public class PixelHostedService : IHostedService
    {
        private readonly PixelController pixelService;

        public PixelHostedService(PixelController pixelService)
        {
            this.pixelService = pixelService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.pixelService.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await this.pixelService.StopAsync(cancellationToken);
        }
    }
}
