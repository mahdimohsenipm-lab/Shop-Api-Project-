using AutoMapper;
using Data.Contracts;
using Entites.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DiscountService.Area.GetDiscountDetail;
using Services.DiscountService.Area.GetDiscountService;
using Services.DiscountService.Area.UpdateDiscountService;
using Services.ViewModel.Area.Model.Request;
using WebFramework.Filter;

namespace StoreTest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiResultFilter]
    [Authorize(Roles = "Admin")]
    public class DiscountController : Controller
    {
        private readonly IGetDiscountDetailService _getDiscountDetailService;
        private readonly IRepository<ProductDiscount> _repository;
        private readonly IUpdateDiscountService _updateDiscountService;
        private readonly IGetDiscountService _getDiscountService;


        public DiscountController(IGetDiscountDetailService getDiscountDetailService,IRepository<ProductDiscount> repository,
           IUpdateDiscountService updateDiscountService,
            IGetDiscountService getDiscountService)
        {
            _getDiscountDetailService=getDiscountDetailService;
            _repository = repository;
            _updateDiscountService=updateDiscountService;
            _getDiscountService=getDiscountService;

        }
        public IActionResult Index(int? page)
        {
            var discounts = _getDiscountService.Execute(page);
            return View(discounts);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int Id ,string? status,CancellationToken cancellationToken)
        {
            bool indexs = false;
            if (status == "true")
            {
                indexs = true;
            }
            var result = await _getDiscountDetailService.Execute(Id, indexs, cancellationToken);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var result= await _repository.GetByIdAsync(cancellationToken,Id);

            await _repository.DeleteAsync(result,cancellationToken);

            return Ok();
        
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateDiscountDto discountDto, CancellationToken cancellationToken)
        {

            await _updateDiscountService.Execute(discountDto,cancellationToken);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> StatusChenge(int Id,CancellationToken cancellationToken)
        {
            var result= await _repository.GetByIdAsync(cancellationToken,Id);
            result.IsActive = !result.IsActive;
            await _repository.UpdateAsync(result,cancellationToken);

            return Ok();
        }
    }
}
