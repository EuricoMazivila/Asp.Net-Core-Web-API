using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.ApplicationPermissions.Queries.RequestModels;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Features.ApplicationPermissions.Queries.Handlers
{
    public class ListPermissionsHandler : IRequestHandler<ListPermissionsQuery, IReadOnlyList<ApplicationPermission>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListPermissionsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<IReadOnlyList<ApplicationPermission>> Handle(ListPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _unitOfWork.Repository<ApplicationPermission>().GetAllAsync();

            return permissions;
        }
    }
}