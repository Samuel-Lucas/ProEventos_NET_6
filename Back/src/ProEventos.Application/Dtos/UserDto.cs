namespace ProEventos.Application.Dtos
{
    public class UserDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PrimeiroNome { get; set; } = null!;
        public string UltimoNome { get; set; } = null!;
    }
}