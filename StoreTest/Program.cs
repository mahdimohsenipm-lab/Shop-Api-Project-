using Common;
using Entites.Users;
using Microsoft.AspNetCore.Identity;
using NLog;
using NLog.Web;
using WebFramework.Configuration;
using WebFramework.CustomMapping;
using WebFramework.Middleware;
namespace StoreTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Admin/Authentication/Login";
                options.AccessDeniedPath = "/Admin/Authentication/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddAuthorization();

            builder.Services.AddControllersWithViews();

            var siteSettings = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();

            builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));

            builder.Services.AddOpenApi();

            builder.Services.AddDateBase(builder.Configuration);

            builder.Services.Addservices();

            builder.Host.UseNLog();

            builder.Services.CustomIdentity(siteSettings.IdentitySetting);

            builder.Services.AddCustomMapping();

            builder.Services.AddJwtAuthentication(siteSettings.JwtSettings);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();
                await IdentitySeeder.SeedAsync(userManager, roleManager);
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseHttpsRedirection();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseRouting();

            //app.UseSession();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCustomExceptionMiddleware();

            app.MapControllerRoute(
                            name: "areas",
                            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllers();

            LogManager.Setup().LoadConfigurationFromFile("nlog.config");

            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
