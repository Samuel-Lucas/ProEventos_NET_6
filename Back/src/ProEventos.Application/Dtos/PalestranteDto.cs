namespace ProEventos.Application.Dtos
{
    public class PalestranteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string MiniCurriculo { get; set; } = null!;
        public string ImagemUrl { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; } = null!;
        public IEnumerable<PalestranteDto> Palestrantes { get; set; }  = null!;
    }
}