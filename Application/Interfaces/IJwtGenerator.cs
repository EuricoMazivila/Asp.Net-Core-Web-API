using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IJwtGenerator
    {
        Task<string> CreateToken(AppUser user, List<ApplicationPermission> applicationPermissions);
    }
}