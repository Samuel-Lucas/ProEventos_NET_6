namespace ProEventos.Domain.Identity
{
    public class Role
    {
        public IEnumerable<UserRole> UserRoles { get; set; } = null!;
    }
}