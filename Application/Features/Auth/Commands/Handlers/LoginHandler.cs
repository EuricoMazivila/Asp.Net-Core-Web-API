using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Errors;
using Application.Features.Auth.Commands.RequestModels;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Auth.Commands.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginDto>
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly DataContext _context;

        public LoginHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, 
            IJwtGenerator jwtGenerator, DataContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _context = context;
        }
        
        public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Where(x => x.Email == request.Email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ApiException(HttpStatusCode.Unauthorized, "Fail to login, user not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                return new LoginDto
                {
                    FullName = user.FullName,
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _jwtGenerator.CreateToken(user)
                };
            }
            
            throw new ApiException(HttpStatusCode.Unauthorized,"Login Failed");
        }
    }
}