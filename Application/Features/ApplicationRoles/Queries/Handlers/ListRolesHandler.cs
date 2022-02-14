using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.ApplicationRoles.Queries.RequestModels;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Features.ApplicationRoles.Queries.Handlers
{
    public class ListRolesHandler : IRequestHandler<ListRolesQuery, IReadOnlyList<ApplicationRole>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListRolesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<IReadOnlyList<ApplicationRole>> Handle(ListRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.Repository<ApplicationRole>().GetAllAsync();

            return roles;
        }
    }
}