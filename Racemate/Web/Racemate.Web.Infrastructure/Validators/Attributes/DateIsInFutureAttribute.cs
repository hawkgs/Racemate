namespace Racemate.Web.Infrastructure.Validators.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Property)]
    public class DateIsInFutureAttribute : ValidationAttribute, IClientValidatable
    {
        public DateIsInFutureAttribute()
        {
            this.ErrorMessage = "{0} should be in future";
        }

        public override bool IsValid(object value)
        {
            var valueAsDateTime = value as DateTime?;

            if (valueAsDateTime == null)
            {
                throw new ArgumentException("The property is not of DateTime format or is null");
            }
            if (valueAsDateTime > DateTime.Now)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule()
            {
                ValidationType = "DateIsInFuture",
                ErrorMessage = this.ErrorMessage
            };
        }
    }
}
