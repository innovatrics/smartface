using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmartFace.Cli.Common;

namespace SmartFace.Cli.Core.Domain.WatchlistMember.Impl
{
    public class RegistrationImgExtensionValidator : ValidationAttribute
    {
        public RegistrationImgExtensionValidator() : base($"File must be {Constants.JPEG.ToUpper()} or {Constants.PNG.ToUpper()}")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var photo = (string)value;
            if (IsRegistrationPhotoExtensionValid(photo))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public static bool IsRegistrationPhotoExtensionValid(string photo)
        {
            var allowedExtensions = new[] { Constants.JPEG, Constants.JPG, Constants.PNG };
            return allowedExtensions.Contains(photo.Split('.').Last().ToLowerInvariant());
        }
    }
}
