using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NeoPixelController;
using NeoPixelController.Logic.Effects;
using NeoPixelServer.ViewModels;

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
            var effects = effectController.GetEffects();
            List<BaseViewModel> viewModels = new List<BaseViewModel>();
            foreach (var effect in effects)
            {
                if (effect is CurveEffect curveEffect)
                {
                    viewModels.Add(new CurveEffectViewModel()
                    {
                        Id = curveEffect.Id,
                        EffectLength = curveEffect.EffectLength,
                        EffectSpeed = curveEffect.EffectSpeed,
                        NumberOfPixels = curveEffect.NumberOfPixels,
                        PixelStartPosition = curveEffect.PixelStartPosition,
                        Name = curveEffect.Name
                    });
                }
            }
            return View(new PixelViewModel()
            {
                BaseViewModels = viewModels
            });
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