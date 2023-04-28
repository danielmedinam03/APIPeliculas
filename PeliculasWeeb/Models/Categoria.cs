using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class Categoria
    {
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
