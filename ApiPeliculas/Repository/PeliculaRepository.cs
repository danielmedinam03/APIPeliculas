using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;

namespace ApiPeliculas.Repository
{
    public class PeliculaRepository : BaseGenericRepository<Pelicula>, IPeliculaRepository
    {
        public PeliculaRepository(Context context) : base(context)
        {
        }
    }
}
