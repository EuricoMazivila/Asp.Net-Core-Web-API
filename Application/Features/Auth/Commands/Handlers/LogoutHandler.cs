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

namespace Application.Features.Auth.Commands.Handlers
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
                throw new ApiException(HttpStatusCode.NotFound, "Fail to logout, user not found");
            }

            return new LoginDto
            {
                Token = ""
            };
        }
    }
}