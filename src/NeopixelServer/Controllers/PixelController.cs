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
        private readonly EffectController effectController;

        public PixelController(EffectController effectController)
        {
            this.effectController = effectController;
        }

        public IActionResult Index()
        {
            effectController.GetEffects();
            return View();
        }

        public IActionResult Curve()
        {
            //pixelService.AnimationType = NeoPixelController.Model.AnimationType.HermitCurve;
            return Redirect("index");
        }

        public IActionResult SetColor()
        {
            //pixelService.AnimationType = NeoPixelController.Model.AnimationType.SingleColor;
            return Redirect("index");
        }

        public IActionResult Pulse()
        {
            //pixelService.AnimationType = NeoPixelController.Model.AnimationType.Pulse;
            return Redirect("index");
        }
    }
}