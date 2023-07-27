using System.ComponentModel.DataAnnotations;

namespace TodoApi.ViewModels
{
    public class CreateTodoViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [MinLength(3, ErrorMessage = "O campo {0} deve conter no mínimo {1} caracteres")]
        [MaxLength(60, ErrorMessage = "O campo {0} deve conter no máximo {1} caracteres")]
        public required string Title { get; set; }
    }
}