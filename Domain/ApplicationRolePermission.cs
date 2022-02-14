namespace Domain
{
    public class ApplicationRolePermission
    {
        public int Id { get; set; }
        public ApplicationRole ApplicationRole { get; set; }
        public ApplicationPermission ApplicationPermission { get; set; }
    }
}