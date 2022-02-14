using System.Collections.Generic;
using Domain;

namespace Application.DTOs
{
    public class ApplicationRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> ApplicationPermissions { get; set; }
    }
}