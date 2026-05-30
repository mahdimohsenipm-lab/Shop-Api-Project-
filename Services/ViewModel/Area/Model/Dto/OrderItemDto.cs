namespace Services.ViewModel.Area.Model.Dto
{
    public class OrderItemDto
    {
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }

        public bool IsPay { get; set; }
    }
}
