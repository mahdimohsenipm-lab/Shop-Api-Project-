using Data.Contracts;
using Entites.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DiscountCodeService.Area.AddDiscountCode;
using Services.DiscountCodeService.Area.GetDiscountCode;
using Services.ViewModel.Area.Model.Dto;
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
        private readonly IGetDiscountCodeService _getDiscountCodeService;
        private readonly IRepository<DiscountCode> _repository;

        public DiscountCodeController(IAddDiscountCodeService discountCodeService,
            IGetDiscountCodeService getDiscountCodeService, IRepository<DiscountCode> repository)
        {
            _discountCodeService = discountCodeService;
            _getDiscountCodeService = getDiscountCodeService;
            _repository = repository;
        }
        [HttpGet]
        public IActionResult Index(int? page)
        {
            var result = _getDiscountCodeService.Execute(page);
           
            return View(result);
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
        [HttpPost]

        public async Task<IActionResult> Delete(int Id , CancellationToken cancellationToken)
        {
            var discountcode = await _repository.GetByIdAsync(cancellationToken,Id);

            await _repository.DeleteAsync(discountcode,cancellationToken);
            return Ok();
        }
    }
}
