using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;

namespace PeliculasWeb.Repository
{
    public class UsuarioRepository : BaseRepository<UsuarioU>, IUsuarioRepository
    {
        public UsuarioRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }
    }
}
