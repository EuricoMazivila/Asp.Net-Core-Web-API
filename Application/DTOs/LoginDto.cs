namespace Application.DTOs
{
    public class LoginDto
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public ApplicationRoleDto ApplicationRoleDto { get; set; }
    }
}