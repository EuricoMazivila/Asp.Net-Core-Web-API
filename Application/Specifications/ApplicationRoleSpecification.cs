using Domain;

namespace Application.Specifications
{
    public class ApplicationRoleSpecification : BaseSpecification<ApplicationRole>
    {
        public ApplicationRoleSpecification(string name)
        : base(x=>x.Name.ToUpper() == name.ToUpper())
        {
            
        }
    }
}