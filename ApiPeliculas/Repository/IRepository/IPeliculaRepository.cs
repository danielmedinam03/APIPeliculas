using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository.IBaseRepository;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IPeliculaRepository : IBaseGenericRepository<Pelicula>
    {
    }
}
