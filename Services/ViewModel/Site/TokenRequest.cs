using System.ComponentModel.DataAnnotations;

namespace Services.ViewModel.Site
{
    public class LoginRequest
    {
       
        [Required(ErrorMessage = "نام کاربری الزامی است")]

        public string Email { get; set; }
        [Required(ErrorMessage = "رمز عبور الزامی است")]

        public string Password { get; set; }
     
    }
}
