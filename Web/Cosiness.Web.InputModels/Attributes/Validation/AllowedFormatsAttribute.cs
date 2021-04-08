namespace Cosiness.Web.InputModels.Attributes.Validation
{
    using Microsoft.AspNetCore.Http;

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    public class AllowedFormatsAttribute : ValidationAttribute
    {
        private readonly string[] _formats;

        public AllowedFormatsAttribute(string[] formats)
        {
            _formats = formats;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is not null)
            {
                var format = Path.GetExtension(file.FileName);
                if (!_formats.Contains(format.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
            => "Not allowed format!";
    }
}
