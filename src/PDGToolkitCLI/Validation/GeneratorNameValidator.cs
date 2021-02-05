using System.ComponentModel.DataAnnotations;
using System.Linq;
using PDGToolkitCore.API;

namespace PDGToolkitCLI.Validation
{
    class IsValidGeneratorName : ValidationAttribute
    {
        public IsValidGeneratorName()
            : base(
"The value for {0} must be one of available generators, option -l | --list to see all available generators"
                )
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var generators = GeneratorService.GetAllGenerators();
            if (value == null || !generators.Contains(value))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
    
}
