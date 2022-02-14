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
    public class AddRoleHandler : IRequestHandler<AddRoleCommand, ApplicationRole>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddRoleHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApplicationRole> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var roleSpec = new ApplicationRoleSpecification(request.Name);
            var roleCheck = await _unitOfWork.Repository<ApplicationRole>().GetEntityWithSpecAsync(roleSpec);

            if (roleCheck != null)
            {
                throw new ApiException(HttpStatusCode.Conflict, 
                    "Fail to add new Role, this role exist in database");
            }

            var role = new ApplicationRole
            {
                Name = request.Name,
                Description = request.Description
            };
            
            _unitOfWork.Repository<ApplicationRole>().AddAsync(role);

            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, "Fail to add new Role");
            }
            
            return role;
        }
    }
}