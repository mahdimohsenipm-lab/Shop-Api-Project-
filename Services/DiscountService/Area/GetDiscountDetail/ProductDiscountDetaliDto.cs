namespace Services.DiscountService.Area.GetDiscountDetail
{
    public class ProductDiscountDetaliDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; }


        public int ProductPrice { get; set; }


        public int? AmountDiscount { get; set; }

        public int? PercentageDiscount { get; set; }

        public bool index { get; set; }
        public decimal FinalPrice { get; set; }
        public DateTimeOffset EndTime { get; set; }




    }
}
