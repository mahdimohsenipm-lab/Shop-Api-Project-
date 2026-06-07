using Common;
using Common.Exceptions;
using Common.Utilities;
using Data;
using Data.Contracts;
using Data.Repositories;
using Entites.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Services.CategoryService.Area.GetProductByCatId;
using Services.CategoryService.Site.GetCategoryForSite;
using Services.DiscountCodeService.Area.AddDiscountCode;
using Services.DiscountService.Area.AddDiscountService;
using Services.DiscountService.Area.GetDiscountDetail;
using Services.DiscountService.Area.GetDiscountService;
using Services.DiscountService.Area.UpdateDiscountService;
using Services.Jwt;
using Services.OrderService.Area.DeleteRequestPay;
using Services.OrderService.Area.GetPayRequestAdmin;
using Services.OrderService.Area.GetRequestPayAdminDetail;
using Services.OrderService.Site.AddOrder;
using Services.OrderService.Site.AddPayRequest;
using Services.OrderService.Site.GetOrder;
using Services.OrderService.Site.GetPayRequest;
using Services.OrderService.Site.PaymentVerification;
using Services.OrderService.Site.UpdateTotalPrice;
using Services.ProductsServices.Commands.AddProduct;
using Services.ProductsServices.Commands.UpdateProduct;
using Services.ProductsServices.Querys.GetProductDetail;
using Services.ProductsServices.Querys.GetProductDetailSite;
using Services.ProductsServices.Querys.GetProductSite;
using System.Net;
using System.Security.Claims;
using System.Text;
using WebFramework.Api;
using ZarinPal.Pay.IService;
using ZarinPal.Pay.Service;

namespace WebFramework.Configuration
{
    public static class ServicCollectionExtention
    {
        public static void Addservices(this IServiceCollection services)
        {
           services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
           services.AddScoped<IUserRepository, UserRepository>();
           services.AddScoped<IJwtServic, JwtServic>();
           services.AddScoped<IProductsRepository, ProductsRepository>();
           services.AddScoped<ICategoryRepository, CategoryRepository>();
           services.AddScoped<IAddProductService, AddProductService>();
           services.AddScoped<IGetProductDetailService, GetProductDetailService>();
           services.AddScoped<IUpdateProductService, UpdateProductService>();
           services.AddScoped<IGetProductSiteService, GetProductSiteService>();
           services.AddScoped<IAddOrderService, AddOrderService>();
           services.AddScoped<IGetOrderService, GetOrderService>();
           services.AddScoped<IAddRequestPay, AddRequestPay>();
           services.AddScoped<IGetRequestServic, GetRequestServic>();
           services.AddScoped<IPaymentVerificationService, PaymentVerificationService>();
           services.AddScoped<IZarinPalPaymentServices, ZarinPalPaymentServices>();
           services.AddScoped<IGetPayRequestAdminService, GetPayRequestAdminService>();
           services.AddScoped<IGetRequestPayAdminDetailService, GetRequestPayAdminDetailService>();
           services.AddScoped<IUpdateTotalPriceService, UpdateTotalPriceService>();
           services.AddScoped<IDeleteRequestPayService, DeleteRequestPayService>();
           services.AddScoped<IGetCategoryForSiteService, GetCategoryForSiteService>();
           services.AddScoped<IGetProductByCatIdService, GetProductByCatIdService>();
           services.AddScoped<IGetProductDetailSiteService, GetProductDetailSiteService>();
           services.AddScoped<IAddDiscountService, AddDiscountService>();
           services.AddScoped<IGetDiscountDetailService, GetDiscountDetailService>();
           services.AddScoped<IUpdateDiscountService, UpdateDiscountService>();
           services.AddScoped<IGetDiscountService, GetDiscountService>();
           services.AddScoped<IAddDiscountCodeService, AddDiscountCodeService>();













        }
        public static void AddDateBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => 
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });
        }

        public static void AddMinimalMvc(this IServiceCollection services)
        {
            services.AddMvcCore(options =>
            {
                options.Filters.Add(new AuthorizeFilter());

                //Like [ValidateAntiforgeryToken] attribute but dose not validatie for GET and HEAD http method
                //You can ingore validate by using [IgnoreAntiforgeryToken] attribute
                //Use this filter when use cookie 
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                //options.UseYeKeModelBinder();
            })
          .AddApiExplorer()
          .AddAuthorization()
          .AddFormatterMappings()
          .AddDataAnnotations()
          .AddJsonOptions(options =>
          {
              options.JsonSerializerOptions.PropertyNamingPolicy = null;
          })
            .AddCors();

          
        }


        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings settings)
        {
            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                var secretKey = Encoding.UTF8.GetBytes(settings.SecretKey);
                var Encryptkey = Encoding.UTF8.GetBytes(settings.Encryptkey);
                var validationParametr = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,// default: 5 min
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidAudience = settings.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    RoleClaimType = ClaimTypes.Role,

                    TokenDecryptionKey = new SymmetricSecurityKey(Encryptkey),
                };
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParametr;
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("Authentication failed.", context.Exception);

                        if (context.Exception != null)
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var applicationSignInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                        //var UpdateSecurityStampService = context.HttpContext.RequestServices.GetRequiredService<IUpdateSecurityStampServices>();


                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                        if (!securityStamp.HasValue())
                            context.Fail("This token has no secuirty stamp");

                        //Find user and token from database and perform your custom validation
                        var userId = claimsIdentity.GetUserId<int>();
                        var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);

                        //if (user.SecurityStamp != securityStamp)
                        //    context.Fail("Token secuirty stamp is not valid.");

                        var validatedUser = await applicationSignInManager.ValidateSecurityStampAsync(context.Principal);
                        if (validatedUser == null)
                            context.Fail("Token secuirty stamp is not valid.");

                        //if (!user.IsActive)
                        //    context.Fail("User is not active.");

                        await userRepository.LastLoginDate(user, context.HttpContext.RequestAborted);
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = new
                        {
                            message = "You are unauthorized to access this resource."
                        };
                        await context.Response.WriteAsJsonAsync(result);
                    }




                };

            });




        }
    }
}
