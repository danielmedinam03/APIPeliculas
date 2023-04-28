using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;

namespace ApiPeliculas.Repository
{
    public class AppUsuarioRepository : BaseGenericRepository<AppUsuario>, IAppUsuarioRepository
    {
        public AppUsuarioRepository(Context context) : base(context)
        {
        }
    }
}
