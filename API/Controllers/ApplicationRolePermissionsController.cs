using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Features.ApplicationRolePermissions.Commands.RequestModels;
using Application.Features.ApplicationRolePermissions.Queries.RequestModels;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ApplicationRolePermissionsController : BaseController
    {
        [HttpPost]
        [Authorize(Roles = "CreateRolePermissions")]
        public async Task<ActionResult<ApplicationRolePermission>> CreateRolePermissions(CreateRolePermissionCommand command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpGet]
        [Authorize(Roles = "ListRolePermissions")]
        public async Task<IReadOnlyList<ApplicationRoleDto>> ListRolePermissions()
        {
            return await Mediator.Send(new ListRolePermissionsQuery());
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "RemoveRolePermissions")]
        public async Task<ActionResult<ApplicationRolePermission>> RemoveRolePermissions(int id)
        {
            return await Mediator.Send(new RemoveRolePermissionCommand{Id = id});
        }
    }
}