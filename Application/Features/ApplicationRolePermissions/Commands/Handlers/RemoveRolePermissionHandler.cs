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
    public class RemoveRolePermissionHandler : IRequestHandler<RemoveRolePermissionCommand, ApplicationRolePermission>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveRolePermissionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<ApplicationRolePermission> Handle(RemoveRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var rolePermissionSpec = new ApplicationRolePermissionsSpecification(request.Id);
            var rolePermission = await _unitOfWork.Repository<ApplicationRolePermission>()
                .GetEntityWithSpecAsync(rolePermissionSpec);

            if (rolePermission == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Role permission not exists");
            }
            
            _unitOfWork.Repository<ApplicationRolePermission>().Delete(rolePermission);
            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, "Problem to remove role permission");
            }

            return rolePermission;
        }
    }
}