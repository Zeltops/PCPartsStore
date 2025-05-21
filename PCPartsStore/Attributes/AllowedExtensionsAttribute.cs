using System.ComponentModel.DataAnnotations;

namespace PCPartsStore.Attributes;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _allowedExtensions;
    private const string ErrorMessage = "This photo extension is not allowed!";

    public AllowedExtensionsAttribute(string[] allowedExtensions)
    {
        _allowedExtensions = allowedExtensions;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!_allowedExtensions.Contains(extension.ToLower()))
            {
                return new ValidationResult(ErrorMessage);
            }
        }
        
        return ValidationResult.Success;
        
      
    }
}