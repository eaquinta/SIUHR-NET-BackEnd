using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.Application.Common.Validations
{
    public class NumericLessThanAttribute : ValidationAttribute, IClientValidatable
    {
        private const string lessThanErrorMessage = "{0} debe ser menor que {1}.";
        private const string lessThanOrEqualToErrorMessage = "{0} debe ser menor o igual que {1}.";
        public string OtherProperty { get; private set; }
        public string OtherPropertyName { get; private set; }

        private bool allowEquality;

        public bool AllowEquality
        {
            get { return this.allowEquality; }
            set
            {
                this.allowEquality = value;
                // Establecer el mensaje de Error  
                this.ErrorMessage = (value ? lessThanOrEqualToErrorMessage : lessThanErrorMessage);
            }
        }

        public NumericLessThanAttribute(string otherProperty, string otherPropertyName = null)
            : base(lessThanErrorMessage)
        {
            if (otherProperty == null) { throw new ArgumentNullException("otherProperty"); }
            this.OtherProperty = otherProperty;
            this.OtherPropertyName = otherPropertyName;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, name, this.OtherPropertyName ?? this.OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult(String.Format("No se logró encontrar encontrar el campo {0}.", OtherProperty));
            }

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            decimal decValue;
            decimal decOtherPropertyValue;

            //  Verifificar que la propiedad es numerica
            if (!decimal.TryParse(value.ToString(), out decValue))
            {
                return new ValidationResult(String.Format("{0} debe ser un valor numérico.", validationContext.DisplayName));
            }

            // Verificar que la otra propiedad es un valor numérico
            if (!decimal.TryParse(otherPropertyValue.ToString(), out decOtherPropertyValue))
            {
                return new ValidationResult(String.Format("{0} debe ser un valor numérico.", OtherProperty));
            }

            // Verificar igualdad si es viable
            if (AllowEquality && decValue == decOtherPropertyValue)
            {
                return null;
            }
            else if (decValue > decOtherPropertyValue)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }


        // ClientSide
        public static string FormatPropertyForClientValidation(string property)
        {
            if (property == null)
            {
                throw new ArgumentException("El valor no puede estar vacio", "property");
            }
            return "*." + property;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(System.Web.Mvc.ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationNumericLessThanRule(FormatErrorMessage(metadata.DisplayName ?? metadata.PropertyName), FormatPropertyForClientValidation(this.OtherProperty), this.AllowEquality);
            //throw new NotImplementedException();
        }
    }

    public class ModelClientValidationNumericLessThanRule : ModelClientValidationRule
    {
        public ModelClientValidationNumericLessThanRule(string errorMessage, object other, bool allowEquality)
        {
            ErrorMessage = errorMessage;
            ValidationType = "numericlessthan";
            ValidationParameters["other"] = other;
            ValidationParameters["allowequality"] = allowEquality;
        }
    }
}
