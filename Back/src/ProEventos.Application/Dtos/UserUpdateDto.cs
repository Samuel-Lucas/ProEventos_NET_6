namespace ProEventos.Application.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string PrimeiroNome { get; set; } = null!;
        public string UltimoNome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Funcao { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}