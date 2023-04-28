using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiPeliculas.Models.Dtos.CategoriaDTOs
{
    public class CategoriaDTO
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="El nombre es obligatorio")]
        [MaxLength(60, ErrorMessage ="El número máximo de caracteres es de 60")]
        public string Nombre { get; set; }
        //[JsonIgnore]
        //public DateTime FechaCreacion 
        //{
        //    get
        //    {
        //        return DateTime.Now;
        //    }
        //}
    }
}
