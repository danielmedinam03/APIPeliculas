using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos.UsuarioDTOs
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; }

    }
}
