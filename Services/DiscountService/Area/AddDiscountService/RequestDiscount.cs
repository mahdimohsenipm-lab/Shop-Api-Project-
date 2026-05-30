namespace Services.DiscountService.Area.AddDiscountService
{
  
        public class RequestDiscount
        {
            public int ProductId { get; set; }

            public int? Amount { get; set; }

            public int? Percentage { get; set; }

            public DateTimeOffset EndTime { get; set; }


        }
    
}
