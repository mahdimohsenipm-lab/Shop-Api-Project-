using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DiscountCodeService.Area.AddDiscountCode;
using System.Threading.Tasks;
using WebFramework.Filter;

namespace StoreTest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiResultFilter]
    [Authorize(Roles = "Admin")]
    public class DiscountCodeController : Controller
    {
        private readonly IAddDiscountCodeService _discountCodeService;

        public DiscountCodeController(IAddDiscountCodeService discountCodeService)
        {
            _discountCodeService = discountCodeService;
        }
        [HttpPost]
        public async Task<IActionResult> AddDiscountCode([FromBody] RequestAddDiscountCode requestAdd,CancellationToken cancellationToken)
        {
            await _discountCodeService.Execute(requestAdd,cancellationToken);
            return Ok();
        }

        [HttpGet]
        public IActionResult AddDiscountCode()
        {
            return View();
        }
    }
}
