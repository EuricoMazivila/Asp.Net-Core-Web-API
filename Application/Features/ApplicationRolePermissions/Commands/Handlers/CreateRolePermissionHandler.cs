using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Features.ApplicationRolePermissions.Commands.RequestModels;
using Application.Interfaces;
using Application.Specifications;
using Domain;
using MediatR;

namespace Application.Features.ApplicationRolePermissions.Commands.Handlers
{
    public class CreateRolePermissionHandler : IRequestHandler<CreateRolePermissionCommand, ApplicationRolePermission>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateRolePermissionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<ApplicationRolePermission> Handle(CreateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.Repository<ApplicationRole>().GetByIdAsync(request.ApplicationRoleId);
            if (role == null)
            {
                throw new ApiException(HttpStatusCode.NotFound,"Role not found");
            }

            var permission = await _unitOfWork.Repository<ApplicationPermission>().GetByIdAsync(request.ApplicationPermissionId);
            if (permission == null)
            {
                throw new ApiException(HttpStatusCode.NotFound,"Permission not found");
            }
            
            var rolePermissionSpec = new ApplicationRolePermissionsSpecification(role.Name, permission.Name);
            var rolePermissionCheck = await _unitOfWork.Repository<ApplicationRolePermission>()
                .GetEntityWithSpecAsync(rolePermissionSpec);
                
            if (rolePermissionCheck != null)
            {
                throw new ApiException(HttpStatusCode.Conflict, 
                    $"Permission {permission} already add in role {role.Name}");
            }
            
            var rolePermission = new ApplicationRolePermission
            {
                ApplicationPermission = permission,
                ApplicationRole = role
            };
            
            _unitOfWork.Repository<ApplicationRolePermission>().AddAsync(rolePermission);
            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, "Fail to Create Role and Permission");
            }

            return rolePermission;
        }
    }
}