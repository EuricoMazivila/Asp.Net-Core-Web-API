using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Errors;
using Application.Features.Users.Queries.RequestModels;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.Handlers
{
    public class ListAllUsersHandler : IRequestHandler<ListAllUsersQuery,IReadOnlyList<UserDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserAccessor _userAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ListAllUsersHandler(UserManager<AppUser> userManager, IUserAccessor userAccessor, 
        IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _userAccessor = userAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<IReadOnlyList<UserDto>> Handle(ListAllUsersQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Where(x => x.Email == _userAccessor.GetCurrentUserEmail())
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fail to logout, user not found");
            }

            var users = await _unitOfWork.Repository<AppUser>().GetAllAsync();

            return _mapper.Map<IReadOnlyList<AppUser>, IReadOnlyList<UserDto>>(users);
            
        }
    }
}