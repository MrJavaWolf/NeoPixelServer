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
            var effects = effectController.GetEffects().OrderBy(e => e.IsEnabled == false);
            List<BaseViewModel> viewModels = new List<BaseViewModel>();
            foreach (var effect in effects)
            {
                if (effect is CurveEffect curveEffect)
                {
                    viewModels.Add(new CurveEffectViewModel()
                    {
                        Id = curveEffect.Id,
                        IsEnabled = curveEffect.IsEnabled,
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

        [HttpPost]
        public IActionResult UpdateCurveEffect(CurveEffectViewModel curveEffect)
        {
            if (ModelState.IsValid)
            {
                var effects = effectController.GetEffects();
                var oldEffect = effects.Where(e => e.Id == curveEffect.Id).FirstOrDefault();
                if (oldEffect is CurveEffect oldCurveEffect)
                {
                    oldCurveEffect.EffectSpeed = curveEffect.EffectSpeed;
                    oldCurveEffect.IsEnabled = curveEffect.IsEnabled;
                    oldCurveEffect.EffectLength = curveEffect.EffectLength;
                    oldCurveEffect.Name = curveEffect.Name;
                    oldCurveEffect.NumberOfPixels = curveEffect.NumberOfPixels;
                    oldCurveEffect.PixelStartPosition = curveEffect.PixelStartPosition;
                }
            }
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