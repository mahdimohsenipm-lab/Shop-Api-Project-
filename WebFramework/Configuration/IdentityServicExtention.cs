using Common;
using Data;
using Entites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFramework.Configuration
{
    public static class IdentityServicExtention
    {
        public static void CustomIdentity(this IServiceCollection services, IdentitySetting settings)
        {
            services.AddIdentity<User, Role>(option =>
            {
                /////Password Setting
                option.Password.RequireDigit = settings.RequireDigit;
                option.Password.RequiredLength = settings.RequiredLength;
                option.Password.RequireUppercase = settings.RequireUppercase;
                option.Password.RequireLowercase = settings.RequireLowercase;
                option.Password.RequireNonAlphanumeric = settings.RequireNonAlphanumeric;


                /////user setting
                ///
                option.User.RequireUniqueEmail = settings.RequireUniqueEmail;


                ////////////Singnin setting
                /////
                //option.SignIn.RequireConfirmedEmail=false;
                //option.SignIn.RequireConfirmedPhoneNumber=false;


                ///////lokout setting
                /////
                //option.Lockout.MaxFailedAccessAttempts=5;
                //option.Lockout. DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        }
    }
}
