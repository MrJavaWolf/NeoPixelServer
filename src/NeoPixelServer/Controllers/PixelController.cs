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
                        Intensity = curveEffect.Intensity,
                        Id = curveEffect.Id,
                        IsEnabled = curveEffect.IsEnabled,
                        EffectLength = curveEffect.EffectLength,
                        EffectSpeed = curveEffect.EffectSpeed,
                        AreaLength = curveEffect.AreaLength,
                        AreaStartPosition = curveEffect.AreaStartPosition,
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
                var effect = effectController.GetEffect<CurveEffect>(curveEffect.Id);
                if (effect != null)
                {
                    effect.EffectSpeed = curveEffect.EffectSpeed;
                    effect.IsEnabled = curveEffect.IsEnabled;
                    effect.EffectLength = curveEffect.EffectLength;
                    effect.Name = curveEffect.Name;
                    effect.AreaLength = curveEffect.AreaLength;
                    effect.AreaStartPosition = curveEffect.AreaStartPosition;
                    effect.Intensity = curveEffect.Intensity;
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