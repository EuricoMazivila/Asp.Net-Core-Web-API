using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Features.ApplicationRolePermissions.Queries.RequestModels;
using Application.Interfaces;
using Application.Specifications;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.ApplicationRolePermissions.Queries.Handlers
{
    public class ListRolePermissionsHandler : IRequestHandler<ListRolePermissionsQuery, IReadOnlyList<ApplicationRoleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ListRolePermissionsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<IReadOnlyList<ApplicationRoleDto>> Handle(ListRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.Repository<ApplicationRole>().GetAllAsync();
            var rolePermissionsSpec = new ApplicationRolePermissionsSpecification();
            var rolesPermissions = await _unitOfWork.Repository<ApplicationRolePermission>()
                .ListAllWithSpecAsync(rolePermissionsSpec);
            var rolePermissions = new List<ApplicationRoleDto>();
            
            foreach (var role in roles)
            {
                var appRolesPermissions = rolesPermissions
                    .Where(x => x.ApplicationRole.Id == role.Id).ToList();

                var permissions = new List<string>();
                foreach (var appRole in appRolesPermissions)
                {
                    permissions.Add(appRole.ApplicationPermission.Name);
                }

                var appRolePermissions = _mapper.Map<ApplicationRole, ApplicationRoleDto>(role);
                appRolePermissions.ApplicationPermissions = permissions;
                
                rolePermissions.Add(appRolePermissions);
            }

            return rolePermissions;
        }
    }
}