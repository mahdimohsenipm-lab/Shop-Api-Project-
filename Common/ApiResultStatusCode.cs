using System.ComponentModel.DataAnnotations;

namespace WebFramework.Api
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "موفق")]
        Success = 200,

        [Display(Name = "درخواست اشتباه")]
        BadRequest = 400,

        [Display(Name = "پیدا نشد")]
        Notfound = 404,

        [Display(Name = "خطای سرور")]
        ServerError = 500,


        [Display(Name = "خطای در پردازش رخ داده است")]
        LogicError = 501,

        [Display(Name = "لیست خالی است")]
        ListEmpty = 503,


        [Display(Name = "اعبار سنجی ناموفق")]
        UnAuthorized = 600


    }
}
