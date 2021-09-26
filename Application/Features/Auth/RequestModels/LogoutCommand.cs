using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.RequestModels
{
    public class LogoutCommand : IRequest<LoginDto>
    {
    }
}