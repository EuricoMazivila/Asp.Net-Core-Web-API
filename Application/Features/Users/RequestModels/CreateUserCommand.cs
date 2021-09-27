using Application.DTOs;
using MediatR;

namespace Application.Features.Users.RequestModels
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}