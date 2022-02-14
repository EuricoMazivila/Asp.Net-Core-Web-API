using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.ApplicationPermissions.Queries.RequestModels;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ApplicationPermissionsController : BaseController
    {
        [HttpGet]
        [Authorize(Roles = "ListPermissions")]
        public async Task<IReadOnlyList<ApplicationPermission>> ListPermissions()
        {
            return await Mediator.Send(new ListPermissionsQuery());
        }
    }
}