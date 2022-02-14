using Domain;

namespace Application.Specifications
{
    public class ApplicationRolePermissionsSpecification : BaseSpecification<ApplicationRolePermission>
    {
        public ApplicationRolePermissionsSpecification()
        {
            AddInclude(x=>x.ApplicationRole);
            AddInclude(x=>x.ApplicationPermission);
        }
        
        public ApplicationRolePermissionsSpecification(int id)
        : base(x=>x.Id == id)
        {
            AddInclude(x=>x.ApplicationRole);
            AddInclude(x=>x.ApplicationPermission);
        }
        
        public ApplicationRolePermissionsSpecification(string role, string permission)
            : base(x=>x.ApplicationRole.Name.ToUpper() == role.ToUpper() 
                      && x.ApplicationPermission.Name.ToUpper() == permission.ToUpper())
        {
            AddInclude(x=>x.ApplicationRole);
            AddInclude(x=>x.ApplicationPermission);
        }
        
        public ApplicationRolePermissionsSpecification(ApplicationRole role)
        : base(x=>x.ApplicationRole.Id == role.Id)
        {
            AddInclude(x=>x.ApplicationRole);
            AddInclude(x=>x.ApplicationPermission);
        }
    }
}