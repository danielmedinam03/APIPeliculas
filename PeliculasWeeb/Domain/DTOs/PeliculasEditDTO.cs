using PeliculasWeb.Models;
using static PeliculasWeb.Models.Pelicula;

namespace PeliculasWeb.Domain.DTOs
{
    public class PeliculasEditDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
