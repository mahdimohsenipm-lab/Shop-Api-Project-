using Entites.Orders;
using Entites.Users;

namespace Services.ViewModel.Area.Model.Dto
{
    public class RequestPayDetailDto
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public User User { get; set; }

        public  bool IsPay { get; set; }

        public List<OrderItemDto> Items { get; set; }

        public  long RefId { get; set; }
        public Order Order { get; set; }

        public DateTime PayDate { get; set; }

    }
}
