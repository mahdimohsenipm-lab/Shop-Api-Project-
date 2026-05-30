using Microsoft.AspNetCore.Mvc;
using Services.CategoryService.Area.GetProductByCatId;
using Services.CategoryService.Site.GetCategoryForSite;
using WebFramework.Filter;

namespace StoreTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class CategoryController : Controller
    {
        private readonly IGetCategoryForSiteService _getCategoryForSiteService;
        private readonly IGetProductByCatIdService _getProductByCatIdService;

        public CategoryController(IGetCategoryForSiteService getCategoryForSiteService, IGetProductByCatIdService getProductByCatIdService)
        {
            _getCategoryForSiteService = getCategoryForSiteService;
            _getProductByCatIdService = getProductByCatIdService;

        }
        [HttpGet("[action]")]
        public  async Task<IActionResult> Index()
        {
            var category =await _getCategoryForSiteService.Execute();
            return Ok(category);
        }
        [HttpGet("[action]")]

        public async Task<IActionResult> GetProducts(int catid)
        {
            var result =await _getProductByCatIdService.Execute(catid);
            return Ok(result);
        }
    }
}
