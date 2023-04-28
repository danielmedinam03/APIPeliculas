using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos.UsuarioDTOs
{
    public class UsuarioDatoDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
    }
}
