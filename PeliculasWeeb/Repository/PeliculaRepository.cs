using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;

namespace PeliculasWeb.Repository
{
    public class PeliculaRepository : BaseRepository<Pelicula>, IPeliculaRepository
    {
        public PeliculaRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }
    }
}
