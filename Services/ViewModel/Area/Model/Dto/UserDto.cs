using Entites.Users;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WebFramework.Api;

namespace Services.ViewModel.Area.Model.Dto
{
    public class UserDto : IValidatableObject
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsStatic { get; set; }

        public bool IsActive { get; set; }

    
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Equals("test", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("نام کاربری نمیتواند Test باشد", new[] { nameof(Name) });
            if (Password.Equals("123"))
                yield return new ValidationResult("رمز عبور نمیتواند 123 باشد", new[] { nameof(Password) });
        }
    }
   
}
