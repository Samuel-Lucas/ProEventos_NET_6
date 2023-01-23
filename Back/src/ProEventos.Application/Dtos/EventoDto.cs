using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        public string Local { get; set; }

        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"),
         MinLength(3, ErrorMessage = "{0} deve ter no mínimo 4 caracteres"),
         MaxLength(50, ErrorMessage = "{0} deve ter no máximo 50 caracteres")]
        public string Tema { get; set; }

        public int QtdPessoas { get; set; }

        public string ImagemUrl { get; set; }

        public string Telefone { get; set; }

        [EmailAddress(ErrorMessage = "O campo {0} precisa ser um email válido")]

        public string Email { get; set; }
    }
}