using ProEventos.Domain.Identity;

namespace ProEventos.Domain.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Local { get; set; } = null!;
        public DateTime? DataEvento { get; set; }
        public string Tema { get; set; } = null!;
        public int QtdPessoas { get; set; }
        public string ImagemUrl { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public IEnumerable<Lote>? Lotes { get; set; }
        public IEnumerable<RedeSocial>? RedesSociais { get; set; }
        public IEnumerable<PalestranteEvento>? PalestrantesEventos { get; set; }
    }
}