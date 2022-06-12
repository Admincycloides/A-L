using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AnL.Repository.Implementation
{
    public class UserRepository : Repository<UserLogin>, IUser
    {
        private Tan_DBContext _context;
        DbSet<UserLogin> dbSet;
        public UserRepository(Tan_DBContext context) : base(context)
        {
            _context = context;
            dbSet = context.Set<UserLogin>();
        }

        public async Task<UserLogin> GetLogin(string username)
        {
            return await Task.Run(() =>
            {
            var details = dbSet.Where(x => x.Username == username).FirstOrDefault();
            return details;
            });

        }
        public async Task<UserLogin> UpdateOTP(int Otp,string Userid)
        {
            return await Task.Run(() =>
            {

                var details = getLogindetails(Userid);
                details.Otp = Otp;
                details.OtpexpiryDate = DateTime.UtcNow;

                var claims = new[]
                        {
                        new Claim(JwtRegisteredClaimNames.Sub, details.UserId),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, details.Username),
                    };

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Issuer",//_configuration["Jwt:Issuer"],
                    ValidAudience = "Audience",//_configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KqcL7s998JrfFHRP"/*_configuration["Jwt:SecretKey"]*/)),
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KqcL7s998JrfFHRP"/*_configuration["Jwt:SecretKey"]*/));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "Issuer",//_configuration["Jwt:Issuer"],
                    audience: "Audience",//_ _configuration["Jwt:Audience"],
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddHours(24),
                    signingCredentials: creds);

                var tokenHandler = new JwtSecurityTokenHandler();
        //        var claimsPrincipal = new JwtSecurityTokenHandler()
        //.ValidateToken(tokenHandler.WriteToken(token), tokenValidationParameters, out var rawValidatedToken);

        //        var result1 = tokenHandler.ValidateToken(tokenHandler.WriteToken(token), tokenValidationParameters, out var validatedToken);


                details.Token = tokenHandler.WriteToken(token);
                details.TokenExpiryDate = token.ValidTo;
                
                _context.SaveChanges();
                return details;
            });
            
        }
        public UserLogin getLogindetails(string UserID)
        {
            return(dbSet.Where(x => x.UserId == UserID).FirstOrDefault());
        }

    }
}
