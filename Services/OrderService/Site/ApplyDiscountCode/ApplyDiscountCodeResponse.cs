namespace Services.OrderService.Site.ApplyDiscountCode
{
    public class ApplyDiscountCodeResponse
    {
        public decimal TotalPrice { get; set; }

        public decimal DiscountAmount { get; set; }

        public int DiscountPercrntage { get; set; }

        public decimal FinalPrice { get; set; }

        

      
    }
}
