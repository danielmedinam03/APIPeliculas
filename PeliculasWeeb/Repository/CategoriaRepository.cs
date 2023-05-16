using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;

namespace PeliculasWeb.Repository
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }
    }
}
