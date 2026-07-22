using AutoMapper;
using Entites.Orders;
using Entites.Products;
using Entites.Users;
using Microsoft.Extensions.DependencyInjection;
using Services.DiscountCodeService.Area.AddDiscountCode;
using Services.ProductsServices.Commands.AddProduct;
using Services.ProductsServices.Querys.GetProductDetail;
using Services.ProductsServices.Querys.GetProductDetailSite;
using Services.ProductsServices.Querys.GetProductSite;
using Services.ViewModel.Area.Model.Dto;
using Services.ViewModel.Area.Model.Request;
using Services.ViewModel.Site;
using Services.ViewModel.Site.Dto.Comment;
using StoreTest.Areas.Admin.Controllers;
using System.Reflection;


namespace WebFramework.CustomMapping
{
    public static class CustomMappingCunfigurationExtenstion
    {
        public static void AddCustomMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(Confige =>
            {
                Confige.AddCustomMappingProfile();

            });


        }
        public static void AddCustomMappingProfile(this IMapperConfigurationExpression services)
        {
            services.AddCustomMappingProfile(Assembly.GetEntryAssembly());
        }
        public static void AddCustomMappingProfile(this IMapperConfigurationExpression services, params Assembly[] assemblies)
        {
            //var allasembly = assemblies.SelectMany(a => a.ExportedTypes);

            //var list = allasembly.Where(type => type.IsClass && !type.IsAbstract && typeof(IHaveCustomMapping).IsAssignableFrom(type))
            //.Select(type => (IHaveCustomMapping)Activator.CreateInstance(type)); ;


            //var profile = new CustomMappingProfile(list);

            //services.AddProfile(profile);
            services.CreateMap<Product, ProductDto>().ReverseMap();
            services.CreateMap<Product, RequestAddProducts>().ReverseMap();
            services.CreateMap<Product, ResultDtoProduct>().ReverseMap();
            services.CreateMap<Product, ProductSiteDto>().ReverseMap();
            services.CreateMap<User, RequestRegister>().ReverseMap();
            services.CreateMap<User, RequestAddUser>().ReverseMap();
            services.CreateMap<RequestPay, PayRquestDto>().ReverseMap();
            services.CreateMap<RequestPay, RequestPayDetailDto>().ReverseMap();
            services.CreateMap<Category, CategoryDto>().ReverseMap();
            services.CreateMap<Product, ProductDetailDto>().ReverseMap();
            services.CreateMap<ProductFiture, ProductFitureDto>().ReverseMap();
            services.CreateMap<ProductImage, ProductImageDto>().ReverseMap();
            services.CreateMap<DiscountCode, RequestAddDiscountCode>().ReverseMap();
            services.CreateMap<DiscountCode, DiscountCodeDto>().ReverseMap();
            services.CreateMap<DiscountCode, RenewRequest>().ReverseMap();
            services.CreateMap<DiscountCode, RequestUpdateDiscountCode>().ReverseMap();
            services.CreateMap<Comment, RequestAddComment>().ReverseMap();

           services.CreateMap<Comment, CommentDto>()
    .ForMember(dest => dest.FullName,
        opt => opt.MapFrom(src => src.User.FullName));






            //services.CreateMap<DiscountCode, DiscountCodeDto>()
            //    .ForMember(dest => dest.LeftTime,
            //        opt => opt.MapFrom(src => src.EndTime))
            //    .ForMember(dest => dest.IsExpired,
            //        opt => opt.MapFrom(src => src.EndTime <= DateTimeOffset.Now));

            //        var now = DateTimeOffset.Now;
            //        services.CreateMap<DiscountCode, DiscountCodeDto>().ForMember(d => d.IsActive,
            //opt => opt.MapFrom(s =>
            //    s.Count > 0 &&
            //    s.StartTime <= now &&
            //    s.EndTime >= now));



            services.CreateMap<Product, ProductDto>()
    .ForMember(dest => dest.IsDiscount, opt => opt.MapFrom(src =>
        src.ProductDiscounts.Any(x =>
            x.IsActive&&
            x.StartTime <= DateTimeOffset.UtcNow &&
            x.EndTime >= DateTimeOffset.UtcNow
        )
    ));





            //services.CreateMap<Product, ProductSiteDto>()
            //    .ForMember(d => d.FinalPrice,
            //        opt => opt.MapFrom(src =>
            //            src.ProductDiscounts
            //                .Where(x => x.IsActive &&
            //                            x.StartTime <= DateTimeOffset.UtcNow &&
            //                            x.EndTime >= DateTimeOffset.UtcNow)
            //                .OrderByDescending(x => x.StartTime)
            //                .Select(x =>
            //                    (int?)(
            //                        x.Percentage.HasValue
            //                            ? src.Price - (src.Price * x.Percentage.Value / 100)
            //                            : (x.Amount.HasValue
            //                                ? src.Price - x.Amount.Value
            //                                : src.Price)
            //                    )
            //                )
            //                .FirstOrDefault()
            //                ?? src.Price
            //        )
            //    );







            services.CreateMap<Product, ProductSiteDto>()
           .ForMember(dest => dest.Src,
            opt => opt.MapFrom(src => src.ProductImages
           .Select(x => x.Src)
           .FirstOrDefault()));

            services.CreateMap<Category, CategoryDto>()
       .ForMember(dst => dst.HasChildern,
        opt => opt.MapFrom(src => src.SubCategory.Any()))
       .ReverseMap();


           services.CreateMap<Product, ProductDetailDto>()
    .ForMember(dest => dest.ProductFitures, opt => opt.MapFrom(src => src.ProductFitures))
    .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages));


        }


    }
}
