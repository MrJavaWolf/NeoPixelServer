using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
                var editatableProperties = new List<EditablePropertyViewModel>();

                var properties = effect.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var setMethod = property.GetSetMethod();
                    if (setMethod != null)
                    {
                        editatableProperties.Add(new EditablePropertyViewModel()
                        {
                            Id = effect.Id,
                            DisplayName = GetDisplayName(property) ?? property.Name,
                            Description = GetDescription(property) ?? "",
                            PropertyName = property.Name,
                            Value = property.GetValue(effect)
                        });
                    }
                }

                if (effect is CurveEffect curveEffect)
                {
                    viewModels.Add(new CurveEffectViewModel()
                    {
                        Properties = editatableProperties
                    });
                }
                else if (effect is ScrollImageEffect scrollImageEffect)
                {
                    viewModels.Add(new ScrollImageEffectViewModel()
                    {
                        Properties = editatableProperties
                    });
                }
            }
            return View(new PixelViewModel()
            {
                BaseViewModels = viewModels
            });
        }

        private string GetDescription(PropertyInfo property)
        {
            if (Attribute.IsDefined(property, typeof(DescriptionAttribute)))
                return (Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)) as DescriptionAttribute).Description;
            return null;
        }

        private string GetDisplayName(PropertyInfo property)
        {
            if (Attribute.IsDefined(property, typeof(DisplayNameAttribute)))
                return (Attribute.GetCustomAttribute(property, typeof(DisplayNameAttribute)) as DisplayNameAttribute).DisplayName;
            return null;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult UpdateProperty(Guid id, string property, string value)
        {
            if (string.IsNullOrWhiteSpace(property)) return BadRequest($"Missing: 'property' value");
            if (value == null) return BadRequest($"Missing: 'value' value");

            var effect = effectController.GetEffect(id);
            if (effect == null) return BadRequest($"Effect with the given Id was not found. Id: {id}");

            if (string.IsNullOrWhiteSpace(property))
                return BadRequest($"Missing: 'property' value");

            PropertyInfo propertyInfo = effect.GetType().GetProperty(property);
            if (propertyInfo == null) return BadRequest($"Property not found. Property: {property}");

            object newValue = null;
            try
            {
                newValue = Convert.ChangeType(value, propertyInfo.PropertyType);
            }
            catch
            {
                return BadRequest($"Invalid value type, expected: {propertyInfo.PropertyType.ToString()}");
            }

            try
            {
                propertyInfo.SetValue(effect, newValue, null);
            }
            catch (Exception e)
            {
                return BadRequest($"Failed to set value. " + e.ToString());
            }

            return Ok();
        }

    }
}