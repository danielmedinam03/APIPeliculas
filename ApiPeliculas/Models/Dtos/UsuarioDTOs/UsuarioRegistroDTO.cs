using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos.UsuarioDTOs
{
    public class UsuarioRegistroDTO
    {
        [Required(ErrorMessage ="El usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El password es obligatorio")]
        public string Password { get; set; }
        public IEnumerable<string> Rol { get; set; } = new List<string>();

    }
}
