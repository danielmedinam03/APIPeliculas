using Microsoft.AspNetCore.Mvc.Rendering;
using PeliculasWeb.Domain.DTOs;

namespace PeliculasWeb.Models.ViewModels
{
    public class PeliculasEditVM
    {
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }
        public PeliculasEditDTO Pelicula { get; set; }
    }

    public class PeliculasVM
    {
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }
        public Pelicula Pelicula { get; set; }
    }
}
