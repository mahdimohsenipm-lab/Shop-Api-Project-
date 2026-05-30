using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ProductsServices.Querys.GetProductDetail;
using Services.ProductsServices.Querys.GetProductDetailSite;
using Services.ProductsServices.Querys.GetProductSite;
using WebFramework.Filter;

namespace StoreTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    [AllowAnonymous]
    public class ProductsController : Controller
    {
        private readonly IGetProductSiteService getProductSite;
        private readonly IGetProductDetailSiteService _getProductDetailService;

        public ProductsController(IGetProductSiteService getProductSite, IGetProductDetailSiteService getProductDetailService)
        {
            this.getProductSite = getProductSite;  
            _getProductDetailService= getProductDetailService;
        }
        [HttpGet]
        public async Task<ActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await getProductSite.Execute(cancellationToken);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductDetail(int id, CancellationToken cancellationToken)
        {
            var result = await _getProductDetailService.Execute(id,cancellationToken);
            return Ok(result);
        }
    }
}
