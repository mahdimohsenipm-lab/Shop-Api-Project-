namespace Services.OrderService.Site.ApplyDiscountCode
{
    public class ApplyDiscountCodeRequest
    {
        public string Code { get; set; }

        public List<ApplyDiscountItemRequest> Items { get; set; } = new();
    }
}
