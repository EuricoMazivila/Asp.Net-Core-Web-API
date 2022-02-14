using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Errors;
using Application.Features.Auth.Commands.RequestModels;
using Application.Interfaces;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public LoginHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, 
            IJwtGenerator jwtGenerator, DataContext context, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Where(x => x.Email == request.Email)
                .Include(x=>x.ApplicationRole)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ApiException(HttpStatusCode.Unauthorized, "Fail to login, user not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            { 
                var permissions = await _context.ApplicationRolePermissions
                    .Where(x=>x.ApplicationRole.Id == user.ApplicationRole.Id)
                    .Include(x => x.ApplicationPermission)
                    .Include(x => x.ApplicationRole)
                    .Select(x => x.ApplicationPermission)
                    .ToListAsync(cancellationToken);

                ApplicationRoleDto userRolePermissions = new ApplicationRoleDto
                {
                    Description = user.ApplicationRole.Description,
                    Name = user.ApplicationRole.Name,
                    ApplicationPermissions = permissions.Select(x=>x.Description).ToList()
                };
                
                var loginDto = _mapper.Map<AppUser, LoginDto>(user);
                loginDto.Token = await _jwtGenerator.CreateToken(user, permissions);
                loginDto.ApplicationRoleDto = userRolePermissions;
                return loginDto;
            }
            
            throw new ApiException(HttpStatusCode.Unauthorized,"Login Failed");
        }
    }
}