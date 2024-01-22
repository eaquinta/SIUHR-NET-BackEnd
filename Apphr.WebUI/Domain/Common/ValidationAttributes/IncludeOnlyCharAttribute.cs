using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Apphr.Domain.Common.ValidationAttributes
{
    public class IncludeOnlyCharAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _chars;
        public IncludeOnlyCharAttribute(string chars) : base("{0} Contitiene Caracters invalidos.")
        {
            _chars = chars;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                var validCharList = _chars.Split(',');

                if (!validCharList.Contains(valueAsString))
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }
            return ValidationResult.Success;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("chars", _chars);
            rule.ValidationType = "include";
            yield return rule;
        }
    }
}
