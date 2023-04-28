using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoria es obligatorio")]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
