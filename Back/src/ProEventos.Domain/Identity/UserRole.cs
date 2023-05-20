namespace ProEventos.Domain.Identity
{
    public class UserRole
    {
        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}