namespace Services.OrderService.Site.AddOrder
{
    public class RequestAddOrder
    {
        public int UserId { get; set; }

        public int PayRequestId { get; set; }
        public string Address { get; set; }

        public List<RequestProductsOrder> Items { get; set; }

    }
}
