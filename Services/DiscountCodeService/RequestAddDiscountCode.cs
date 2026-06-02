namespace Services.DiscountCodeService
{
    public class RequestAddDiscountCode
    {
        public int? Amount { get; set; }

        public int Count { get; set; }

        public string Code { get; set; }

        public int LimitPrice { get; set; }
        public int? Persentage { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }



    }
}
