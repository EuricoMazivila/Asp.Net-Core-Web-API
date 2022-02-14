using System.Collections.Generic;
using Domain;
using MediatR;

namespace Application.Features.ApplicationPermissions.Queries.RequestModels
{
    public class ListPermissionsQuery : IRequest<IReadOnlyList<ApplicationPermission>>
    {
    }
}