namespace ProEventos.Domain.Models
{
    public class RedeSocial
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string URL { get; set; } = null!;
        public int? EventoId { get; set; }
        public Evento? Evento { get; set; }
        public int? PalestranteId { get; set; }
        public Palestrante? Palestrante { get; set; }
    }
}