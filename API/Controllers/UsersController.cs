using System.Threading.Tasks;
using Application.DTOs;
using Application.Features.Users.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}