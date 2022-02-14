using Domain;
using MediatR;

namespace Application.Features.ApplicationRolePermissions.Commands.RequestModels
{
    public class CreateRolePermissionCommand : IRequest<ApplicationRolePermission>
    {
        public int ApplicationRoleId { get; set; }
        public int ApplicationPermissionId { get; set; }
    }
}