using System.Collections.Generic;
using Domain;
using MediatR;

namespace Application.Features.ApplicationRoles.Queries.RequestModels
{
    public class ListRolesQuery : IRequest<IReadOnlyList<ApplicationRole>>
    {
    }
}