using System.ComponentModel.DataAnnotations;
using System.Linq;
using PDGToolkitCore.API;

namespace PDGToolkitCLI.Validation
{
    internal class IsValidSerialiserName : ValidationAttribute
    {
        private static readonly string CustomErrorMessage = ResponseFormatter.RespondWithCollection(
            "The value for {0} must be one of the available serialisers, currently supported ones are: ",
            InternalComponentService.GetAllSerialisers());
        
        public IsValidSerialiserName() : base(CustomErrorMessage) { }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var serialisers = InternalComponentService.GetAllSerialisers();
            if (value == null || !serialisers.Contains(value))
                return new ValidationResult(FormatErrorMessage(context.DisplayName));

            return ValidationResult.Success;
        }
    }
}