using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModel.Site
{
    public class RequestRegister
    {
        //[Required(ErrorMessage = "نام الزامی است")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        public string Email { get; set; }

        [Required(ErrorMessage = "نام الزامی است")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "رمز عبور الزامی است")]
        public string Password { get; set; }


        [Required(ErrorMessage = "سن الزامی است")]
        public int Age { get; set; }
    }

}
