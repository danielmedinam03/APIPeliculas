using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;

namespace ApiPeliculas.Repository
{
    public class CategoriaRepository : BaseGenericRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(Context context) : base(context)
        {
        }
    }
}
 