using ProEventos.Domain.Identity;

namespace ProEventos.Domain.Models
{
    public class Palestrante
    {
        public int Id { get; set; }
        public string MiniCurriculo { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public IEnumerable<RedeSocial>? RedesSociais { get; set; }
        public IEnumerable<PalestranteEvento>? PalestrantesEventos { get; set; } 
    }
}