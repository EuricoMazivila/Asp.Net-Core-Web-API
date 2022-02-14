using Domain;
using MediatR;

namespace Application.Features.ApplicationRoles.Commands.RequestModels
{
    public class RemoveRoleCommand : IRequest<ApplicationRole>
    {
        public int Id { get; set; }
    }
}