using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace Infrastructure.Security
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly DataContext _context;
        private readonly SymmetricSecurityKey _key;
        
        public JwtGenerator(IConfiguration config, DataContext context)
        {
            _context = context;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:TokenSecret"]));
        }
        
        public async Task<string> CreateToken(AppUser user, List<ApplicationPermission> applicationPermissions)
        {
            if (user == null) throw new ApiException(HttpStatusCode.NotFound, "User is Empty");
            var userExists = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email && !user.Archived);
            if (userExists == null) throw new ApiException(HttpStatusCode.NotFound, "User not found or is Archived");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            foreach (var permission in applicationPermissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, permission.Name));
            }
            
            //generate signing credentials
            var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}