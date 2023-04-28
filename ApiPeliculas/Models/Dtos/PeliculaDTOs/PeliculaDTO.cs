using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos.PeliculaDTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(60, ErrorMessage = "El número máximo de caracteres es de 60")]
        public string Nombre { get; set; }
        public byte[] RutaImagen { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La duración es obligatoria")]
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CategoriaId { get; set; }

    }
}
