namespace ProEventos.Application.Dtos
{
    public class RedeSocialDto
    {
       public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string URL { get; set; } = null!;
        public int? EventoId { get; set; }
        public EventoDto? Evento { get; set; }
        public int? PalestranteId { get; set; }
        public PalestranteDto? Palestrante { get; set; }
    }
}