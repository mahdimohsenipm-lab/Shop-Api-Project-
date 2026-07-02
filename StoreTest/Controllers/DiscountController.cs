using Microsoft.AspNetCore.Mvc;
using Services.OrderService.Site.ApplyDiscountCode;
using WebFramework.Filter;

namespace StoreTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class DiscountController : Controller
    {
        private readonly IApplyDiscountCodeService _applyDiscountCodeService;

        public DiscountController(IApplyDiscountCodeService applyDiscountCodeService)
        {
            _applyDiscountCodeService = applyDiscountCodeService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Apply(ApplyDiscountCodeRequest request,CancellationToken cancellationToken)
        {
           var result=await _applyDiscountCodeService.Execute(request,cancellationToken);
            return Ok(result);
        }
    }
}
