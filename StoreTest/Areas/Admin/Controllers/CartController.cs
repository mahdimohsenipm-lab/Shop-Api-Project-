using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.OrderService.Area.DeleteRequestPay;
using Services.OrderService.Area.GetPayRequestAdmin;
using Services.OrderService.Area.GetRequestPayAdminDetail;
using WebFramework.Filter;

namespace StoreTest.Areas.Admin.Controllers
{
    [ApiResultFilter]
    [Area("Admin")]
    [Authorize(Roles ="Admin")]

    public class CartController : Controller
    {
        private readonly IGetPayRequestAdminService _getPayRequestAdminService;
        private readonly IGetRequestPayAdminDetailService _getRequestPayAdminDetailService;
        private readonly IDeleteRequestPayService _deleteRequestPayService;

        public CartController(IGetPayRequestAdminService getPayRequestAdminService, 
            IGetRequestPayAdminDetailService getRequestPayAdminDetailService, IDeleteRequestPayService deleteRequestPayService)
        {
            _getPayRequestAdminService = getPayRequestAdminService;
            _getRequestPayAdminDetailService= getRequestPayAdminDetailService;
            _deleteRequestPayService = deleteRequestPayService;
        }

        public IActionResult Index(int? page)
        {
            var result=_getPayRequestAdminService.Execute(page);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id) 
        {
            var result =await _getRequestPayAdminDetailService.Execute(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            await _deleteRequestPayService.Execute(id,cancellationToken);
            return Ok();
        }


    }
}
