using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;

namespace ApiPeliculas.Repository
{
    public class UsuarioRepository : BaseGenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(Context context) : base(context)
        {
        }
    }
}
