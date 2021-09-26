using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Features.Auth.RequestModels;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Handlers
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, LoginDto>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<AppUser> _userManager;

        public LogoutHandler(IUserAccessor userAccessor, UserManager<AppUser> userManager)
        {
            _userAccessor = userAccessor;
            _userManager = userManager;
        }
        
        public async Task<LoginDto> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Where(x => x.Email == _userAccessor.GetCurrentUserEmail())
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new WebException("User not Found", 
                    (WebExceptionStatus) HttpStatusCode.NotFound);
            }

            return new LoginDto
            {
                Token = ""
            };
        }
    }
}