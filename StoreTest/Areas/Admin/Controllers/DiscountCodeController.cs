using AutoMapper;
using Data.Contracts;
using Entites.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DiscountCodeService.Area.ActivationService;
using Services.DiscountCodeService.Area.AddDiscountCode;
using Services.DiscountCodeService.Area.GetDiscountCode;
using Services.ViewModel.Area.Model.Request;
using WebFramework.Filter;

namespace StoreTest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiResultFilter]
    [Authorize(Roles = "Admin")]
    public partial class DiscountCodeController : Controller
    {
        private readonly IAddDiscountCodeService _discountCodeService;
        private readonly IGetDiscountCodeService _getDiscountCodeService;
        private readonly IRepository<DiscountCode> _repository;
        private readonly IActivationService _activationService;
        private readonly IMapper _mapper;

        public DiscountCodeController(IAddDiscountCodeService discountCodeService,
            IGetDiscountCodeService getDiscountCodeService, IRepository<DiscountCode> repository,
            IActivationService activationService,IMapper mapper)
        {
            _discountCodeService = discountCodeService;
            _getDiscountCodeService = getDiscountCodeService;
            _repository = repository;
            _activationService = activationService;
            _mapper = mapper;
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

        [HttpPost]

        public async Task<IActionResult> Renew(RenewRequest request, CancellationToken cancellationToken)
        {
            await _activationService.Execute(request,cancellationToken);
            return Ok();
        
        }
        [HttpPost]

        public async Task<IActionResult> Activation(int Id, CancellationToken cancellationToken)
        {
            var result= await _repository.GetByIdAsync(cancellationToken, Id);

            result.IsActive = !result.IsActive;

            await _repository.UpdateAsync(result,cancellationToken);

            return Ok();
        
        }
        [HttpPost]

        public async Task<IActionResult> Edit(RequestUpdateDiscountCode request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByIdAsync(cancellationToken,request.Id);
            var mapdiscount = _mapper.Map(request,result);
            await _repository.UpdateAsync(mapdiscount,cancellationToken);
            return Ok();   
        
        }
    }
}
