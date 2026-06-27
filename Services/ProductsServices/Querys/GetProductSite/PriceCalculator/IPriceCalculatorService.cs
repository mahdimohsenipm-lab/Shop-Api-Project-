using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductsServices.Querys.GetProductSite.PriceCalculator
{
    public interface IPriceCalculatorService
    {
         int Calculate(ProductDiscountDto discount, int Price);
    }

    public class PriceCalculatorService : IPriceCalculatorService
    {
        public int Calculate(ProductDiscountDto discount, int Price)
        {
            var finalPrice = 0;
            if (discount.Amount.Value != 0)
            {
                finalPrice = Price - discount.Amount.Value;
            }
            else
            {
                finalPrice = Price - (Price * discount.Percentage.Value / 100);
            }

            return finalPrice;
        }
    }

    public class ProductDiscountDto
    {

        public int? Amount { get; set; }

        public int? Percentage { get; set; }

    }
}

