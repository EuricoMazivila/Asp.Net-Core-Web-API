using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public sealed class AppUser: IdentityUser
    {
        public string FullName { get; set; }
        public bool Archived { get; set; }
        public ApplicationRole ApplicationRole { get; set; }
    }
}