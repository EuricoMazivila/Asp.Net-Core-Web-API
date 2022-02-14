using System.Collections.Generic;
using Application.DTOs;
using MediatR;

namespace Application.Features.ApplicationRolePermissions.Queries.RequestModels
{
    public class ListRolePermissionsQuery : IRequest<IReadOnlyList<ApplicationRoleDto>>
    {
    }
}