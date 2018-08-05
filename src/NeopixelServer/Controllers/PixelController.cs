using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NeopixelController;

namespace NeopixelServer.Controllers
{
    public class PixelController : Controller
    {
        private readonly NeopixelController.PixelController pixelService;

        public PixelController(NeopixelController.PixelController pixelService)
        {
            this.pixelService = pixelService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Curve()
        {
            pixelService.AnimationType = NeopixelController.Model.AnimationType.HermitCurve;
            return Redirect("index");
        }

        public IActionResult SetColor()
        {
            pixelService.AnimationType = NeopixelController.Model.AnimationType.SingleColor;
            return Redirect("index");
        }

        public IActionResult Pulse()
        {
            pixelService.AnimationType = NeopixelController.Model.AnimationType.Pulse;
            return Redirect("index");
        }
    }
}