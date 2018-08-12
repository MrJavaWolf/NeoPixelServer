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
                        Name = curveEffect.Name,
                        Intensity = curveEffect.Intensity,
                        EffectLength = curveEffect.EffectLength,
                        EffectSpeed = curveEffect.EffectSpeed,
                        AreaLength = curveEffect.AreaLength,
                        AreaStartPosition = curveEffect.AreaStartPosition,
                    });
                }
                else if(effect is ScrollImageEffect scrollImageEffect)
                {
                    viewModels.Add(new ScrollImageEffectViewModel()
                    {
                        Id = scrollImageEffect.Id,
                        IsEnabled = scrollImageEffect.IsEnabled,
                        Name = scrollImageEffect.Name,
                        Intensity = scrollImageEffect.Intensity,
                        Horizontal = scrollImageEffect.Horizontal,
                        Speed= scrollImageEffect.Speed,
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
                    effect.IsEnabled = curveEffect.IsEnabled;
                    effect.Name = curveEffect.Name;
                    effect.Intensity = curveEffect.Intensity;
                    effect.EffectSpeed = curveEffect.EffectSpeed;
                    effect.EffectLength = curveEffect.EffectLength;
                    effect.AreaLength = curveEffect.AreaLength;
                    effect.AreaStartPosition = curveEffect.AreaStartPosition;
                }
            }
            return Redirect(nameof(Index));
        }


        [HttpPost]
        public IActionResult UpdateScrollImageEffect(ScrollImageEffectViewModel scrollImageEffect)
        {
            if (ModelState.IsValid)
            {
                var effect = effectController.GetEffect<ScrollImageEffect>(scrollImageEffect.Id);
                if (effect != null)
                {
                    effect.IsEnabled = scrollImageEffect.IsEnabled;
                    effect.Name = scrollImageEffect.Name;
                    effect.Intensity = scrollImageEffect.Intensity;
                    effect.Speed = scrollImageEffect.Speed;
                    effect.Horizontal = scrollImageEffect.Horizontal;
                }
            }
            return Redirect(nameof(Index));
        }

    }
}