﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Features.Users.Commands.RequestModels;
using Application.Features.Users.Queries.RequestModels;
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

        [HttpGet]
        public async Task<IReadOnlyList<UserDto>> ListAllUsers()
        {
            return await Mediator.Send(new ListAllUsersQuery());
        }
    }
}