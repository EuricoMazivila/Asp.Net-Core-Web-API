using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Errors;
using Application.Features.Users.Commands.RequestModels;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserHandler(UserManager<AppUser> userManager, IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Email {request.Email} " +
                                                                  $"has been used to create another account.");
            }
            
            var role = await _unitOfWork.Repository<ApplicationRole>().GetByIdAsync(request.RoleId);

            if (role == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, 
                    "Fail Role not found!");
            }
            
            user = new AppUser
            {
                Email = request.Email,
                FullName = request.FullName,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true,
                ApplicationRole = role
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