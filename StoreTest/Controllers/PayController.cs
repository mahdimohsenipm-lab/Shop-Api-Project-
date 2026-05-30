using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.OrderService.Site.AddOrder;
using Services.OrderService.Site.AddPayRequest;
using Services.OrderService.Site.GetOrder;
using Services.OrderService.Site.GetPayRequest;
using Services.OrderService.Site.PaymentVerification;
using Services.OrderService.Site.UpdateTotalPrice;
using Services.ViewModel.Site;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebFramework.Filter;


namespace StoreTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class PayController : Controller
    {
        private readonly IAddOrderService _addOrderService;
        private readonly IAddRequestPay _addRequestPayService;
        private readonly IGetRequestServic _getRequestServic;
        private readonly IPaymentVerificationService _paymentVerificationService;
        private readonly ILogger<PayController> _logger;
        private readonly IGetOrderService _getOrderService;
        private readonly IUpdateTotalPriceService _updateTotalPriceService;
        public PayController(
            IGetOrderService getOrderService, IAddOrderService addOrderService,
            IAddRequestPay addRequestPay, IGetRequestServic getRequestServic,
            IPaymentVerificationService paymentVerificationService, ILogger<PayController> logger,IUpdateTotalPriceService updateTotalPriceService)
        {
            _addOrderService = addOrderService;
            _addRequestPayService = addRequestPay;
            _getRequestServic = getRequestServic;
            _paymentVerificationService = paymentVerificationService;
            _logger = logger;
            _getOrderService = getOrderService;
            _updateTotalPriceService = updateTotalPriceService;
        }
      
   


        [HttpPost("[action]")]
        public async Task<IActionResult> Index(RequestAddPay requestAddPay, CancellationToken cancellationToken)
        {
            if (requestAddPay == null) return BadRequest("مقادیر ارسالی معتبر نیست.");

            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId)) return BadRequest("شناسه کاربر نامعتبر است.");


            var requestPay = await _addRequestPayService.Execute(userId, cancellationToken);

            await _addOrderService.Execute(new RequestAddOrder
            {
                Items = requestAddPay.Items,
                Address = requestAddPay.Address,
                PayRequestId = requestPay.Id,
                UserId = userId
            }, cancellationToken);

            var order = _getOrderService.Execute(requestPay.Id);

            var realPrice = order.TotalPrice * 10;

            ////////Add TotalPrice in RequestPay!
          await _updateTotalPriceService.Execute(requestPay.Id,order.TotalPrice,cancellationToken);

            try
            {
                using var client = new HttpClient();
                var requestData = new
                {
                    merchant_id = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
                    amount = realPrice,
                    callback_url = $"https://localhost:7061/api/Pay/Vrifay?guid={requestPay.Guid}",
                    description = "خرید تست",
                };
                var json = JsonSerializer.Serialize(requestData);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/request.json", content);
           

                var responseString = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ZarinpalResponse>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                });
                if (result?.data != null && result.data.code == 100)
                {
                    var url = $"https://sandbox.zarinpal.com/pg/StartPay/{result.data.authority}";
                    return Ok(new { redirectUrl = url });
                }
                return BadRequest(new { success = false, message = "خطای درگاه", detail = responseString });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Zarinpal Exception");
                return BadRequest(new { success = false, message = "خطای غیرمنتظره رخ داد." });
            }
        }
        [HttpGet("[action]")] // اینجا را از HttpPost به HttpGet تغییر دهید
        [AllowAnonymous]
        public async Task<IActionResult> Vrifay(Guid guid, string Authority, string Status)
        {
            if (Status != "OK" || string.IsNullOrEmpty(Authority))
            {
                return Redirect("/sitetamplate/PaymentFailed.html?error=0");
            }
            var requestPay = await _getRequestServic.Execute(guid);
            if (requestPay == null)
            {
                return BadRequest("درخواست پرداخت معتبر یافت نشد.");
            }
            try
            {
                var realTotalPrice = requestPay.Amount * 10;
                using var client = new HttpClient();
                var verifyData = new
                {
                    merchant_id = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
                    amount = realTotalPrice,
                    authority = Authority
                };
                var json = JsonSerializer.Serialize(verifyData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
       
                var response = await client.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/verify.json", content);
                var responseString = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ZarinpalResponse>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
       
                if (result?.data != null && (result.data.code == 100 || result.data.code == 101))
                {
                    long refid = result.data.ref_id;
                    await _paymentVerificationService.Execute(guid, refid, Authority);
                    return Redirect("/sitetamplate/PaymentSuccess.html?refid=" + refid);
                }
                else
                {
                    return Redirect("/sitetamplate/PaymentFailed.html?errors=verification_failed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Vrifay");
                return Redirect("/sitetamplate/PaymentFailed.html?errors=exception");
            }
        }


        public class ZarinpalResponse
        {
            public ZarinpalData data { get; set; }
            public object errors { get; set; } // ممکن است لیست یا آبجکت باشد
        }
        public class ZarinpalData
        {
            public int code { get; set; }
            public string message { get; set; }
            public string authority { get; set; }
            public string fee_type { get; set; }
            public int fee { get; set; }

            public long ref_id { get; set; }
        }



    }
}
        

