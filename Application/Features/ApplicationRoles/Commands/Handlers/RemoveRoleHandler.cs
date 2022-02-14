using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Features.ApplicationRoles.Commands.RequestModels;
using Application.Interfaces;
using Application.Specifications;
using Domain;
using MediatR;

namespace Application.Features.ApplicationRoles.Commands.Handlers
{
    public class RemoveRoleHandler : IRequestHandler<RemoveRoleCommand, ApplicationRole>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveRoleHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<ApplicationRole> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.Repository<ApplicationRole>().GetByIdAsync(request.Id);

            if (role == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Role not found");
            }

            var rolePermissionsSpec = new ApplicationRolePermissionsSpecification(role);
            var rolePermissions = await _unitOfWork.Repository<ApplicationRolePermission>()
                .ListAllWithSpecAsync(rolePermissionsSpec);
            
            if (rolePermissions != null)
            {
                throw new ApiException(HttpStatusCode.BadGateway, "This role contains permissions!");
            }
            
            _unitOfWork.Repository<ApplicationRole>().Delete(role);
            var result = await _unitOfWork.Complete();
            if (result <= 0)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, 
                    $"Problem to remove role");
            }

            return role;
        }
    }
}