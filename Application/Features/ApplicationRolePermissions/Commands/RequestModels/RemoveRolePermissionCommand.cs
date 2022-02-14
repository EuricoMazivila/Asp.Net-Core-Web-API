using Domain;
using MediatR;

namespace Application.Features.ApplicationRolePermissions.Commands.RequestModels
{
    public class RemoveRolePermissionCommand : IRequest<ApplicationRolePermission>
    {
        public int Id { get; set; }
    }
}