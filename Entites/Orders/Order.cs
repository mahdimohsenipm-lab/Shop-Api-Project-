using Entites.Common;
using Entites.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Orders
{
    public class Order : BaseEntity
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public RequestPay RequestPay { get; set; }
        public int RequestPayId { get; set; }
        public string Address { get; set; }
        public long TotalPrice { get; set; }
        public OrderState orderState { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }


     //برای بررسی در متود زرین پال
        public string DiscountCode { get; set; }

    }


    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasOne(o => o.RequestPay)
                           .WithOne(rp => rp.Order)
                           .HasForeignKey<RequestPay>(rp => rp.OrderId);
        }
    }

    public enum OrderState
    {
        [Display(Name = "پرداخت شد")]
        Success = 1,

        [Display(Name = "درحال پرداخت")]
        Processing = 2,

        [Display(Name = "پرداخت نشد")]
        Problem = 3,

    }

    public class RequestPay : BaseEntity
    {
        public Guid Guid { get; set; }
        public bool IsPay { get; set; }
        public long Amount { get; set; }
        public Order Order { get; set; }
        public int? OrderId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string? Authority { get; set; }
        public long RefId { get; set; } = 0;
        public DateTime PayDate { get; set; }
    }
    
}
