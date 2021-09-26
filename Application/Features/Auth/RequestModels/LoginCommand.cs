using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.RequestModels
{
    public class LoginCommand : IRequest<LoginDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}