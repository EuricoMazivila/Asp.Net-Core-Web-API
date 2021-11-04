using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Errors;
using Application.Features.Users.RequestModels;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public CreateUserHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        
        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Email {request.Email} " +
                                                                  $"has been used to create another account.");
            }
            
            user = new AppUser
            {
                Email = request.Email,
                FullName = request.FullName,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return _mapper.Map<AppUser, UserDto>(user);
            }
            
            throw new ApiException(HttpStatusCode.InternalServerError,"Fail to create new user");
        }
    }
}