using Domain;
using MediatR;

namespace Application.Features.ApplicationRoles.Commands.RequestModels
{
    public class AddRoleCommand : IRequest<ApplicationRole>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}