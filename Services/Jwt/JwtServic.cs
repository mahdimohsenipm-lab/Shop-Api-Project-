using Common;
using Entites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Jwt;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Jwt
{
    public class JwtServic : IJwtServic
    {

        private readonly JwtSettings siteSettings;
        private readonly SignInManager<User> signInManager;


        public JwtServic(IOptionsSnapshot<SiteSettings> siteSettings, SignInManager<User> signInManager)
        {
            this.siteSettings = siteSettings.Value.JwtSettings;
            this.signInManager = signInManager;
        }
        public async Task<string> GenerateAsync(User user)
        {
            var SecretKey = Encoding.UTF8.GetBytes(siteSettings.SecretKey);
            var Encryptkey = Encoding.UTF8.GetBytes(siteSettings.Encryptkey);
            var Claims = await getClaimsAsync(user);
            var signingcredentials = new SigningCredentials(new SymmetricSecurityKey(SecretKey), SecurityAlgorithms.HmacSha256Signature);
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(Encryptkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
            var Descriptor = new SecurityTokenDescriptor()
            {
                Issuer = siteSettings.Issuer,

                Audience = siteSettings.Audience,

                Expires = DateTime.Now.AddDays(siteSettings.ExpirationMinutes),

                NotBefore = DateTime.Now.AddMinutes(siteSettings.NotBeforeMinutes),

                IssuedAt = DateTime.Now,

                SigningCredentials = signingcredentials,

                EncryptingCredentials = encryptingCredentials,

                Subject = new ClaimsIdentity(Claims),

            };

            var TokenHandeler = new JwtSecurityTokenHandler();

            var SecurityToken = TokenHandeler.CreateToken(Descriptor);

            var JWT = TokenHandeler.WriteToken(SecurityToken);

            return JWT;

        }
        private async Task<IEnumerable<Claim>> getClaimsAsync(User user)
        {
            //JwtRegisteredClaimNames.Sub
            
           
            var result = await signInManager.ClaimsFactory.CreateAsync(user);
            //var claimsidentityoptions = new ClaimsIdentityOptions().SecurityStampClaimType;
            //var list = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name,user.UserName),
            //    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            //    new Claim(claimsidentityoptions,user.SecurityStamp.ToString()),



            //};


            //foreach (var Role in roles)
            //{
            //    list.AddIdentity(new Claim(ClaimTypes.Role, Role.Name));

            //}

            return result.Claims;
        }
    }
}
