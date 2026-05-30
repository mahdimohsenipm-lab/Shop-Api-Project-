using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebFramework.Filter;

namespace StoreTest.Controllers
{
    [Authorize]
    [ApiResultFilter]
    public class OrderController : Controller
    {
        public IActionResult AddOrder()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok();
        }
    }
}
