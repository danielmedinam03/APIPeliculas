using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class UsuarioM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "el usuario es obligatorio")]
        public string userName { get; set; }
        [StringLength(12,MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 12 caracteres")]
        public string Password { get; set; }
        public string? token { get; set; }
    }
}
