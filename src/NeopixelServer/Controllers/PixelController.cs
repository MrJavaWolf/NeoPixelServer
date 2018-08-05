using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NeoPixelController;

namespace NeoPixelServer.Controllers
{
    public class PixelController : Controller
    {
        private readonly NeoPixelController.PixelController pixelService;

        public PixelController(NeoPixelController.PixelController pixelService)
        {
            this.pixelService = pixelService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Curve()
        {
            pixelService.AnimationType = NeoPixelController.Model.AnimationType.HermitCurve;
            return Redirect("index");
        }

        public IActionResult SetColor()
        {
            pixelService.AnimationType = NeoPixelController.Model.AnimationType.SingleColor;
            return Redirect("index");
        }

        public IActionResult Pulse()
        {
            pixelService.AnimationType = NeoPixelController.Model.AnimationType.Pulse;
            return Redirect("index");
        }
    }
}