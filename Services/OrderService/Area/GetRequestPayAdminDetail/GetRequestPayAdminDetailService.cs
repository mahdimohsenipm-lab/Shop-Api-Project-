using AutoMapper;
using Data.Contracts;
using Entites.Orders;
using Microsoft.EntityFrameworkCore;
using Services.ViewModel.Area.Model.Dto;

namespace Services.OrderService.Area.GetRequestPayAdminDetail
{
    public class GetRequestPayAdminDetailService : IGetRequestPayAdminDetailService
    {
        private readonly IRepository<RequestPay> _repository;
        private readonly IMapper mapper;
        private readonly IProductsRepository _productsRepository;

        public GetRequestPayAdminDetailService(IRepository<RequestPay>repository,IMapper mapper,IProductsRepository productsRepository)
        {
            _repository=repository;
            this.mapper=mapper;
            _productsRepository = productsRepository;
        }
        public async Task<RequestPayDetailDto> Execute(int requestPayId)
        {
            var result =await _repository.Table.Where(x => x.Id == requestPayId)
                .Include(x => x.User)
                .Include(x => x.Order)
                .ThenInclude(x => x.OrderDetails).ThenInclude(x=>x.Product).IgnoreQueryFilters()
                .FirstOrDefaultAsync();

            if (result==null)
            {
                throw new Exception("فاکتور یافت نشد");
                
            }
            //var orderdetail = result.Order.OrderDetails;

            var items = result.Order.OrderDetails.Select(x=> new OrderItemDto
            {
                Brand=x.ProductBrand,
                Count=x.Count,
                Price=x.Price,
                ProductName=x.ProductName
               
                

            }).ToList();

            

            if (items==null)
            {
                throw new Exception("فاکتور یافت نشد");
            }

            var newresult = mapper.Map<RequestPayDetailDto>(result);
            
            newresult.Items=items;
            newresult.RefId = result.RefId;
            newresult.IsPay = result.IsPay;
            return  newresult;
        }
    }
}
