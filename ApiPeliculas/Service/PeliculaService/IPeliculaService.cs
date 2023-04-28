using ApiPeliculas.Models;
using ApiPeliculas.Service.IBaseService;

namespace ApiPeliculas.Service.PeliculaService
{
    public interface IPeliculaService : IDeleteService<Pelicula>, ICreateService<Pelicula>,
                                         IUpdateService<Pelicula>, IGetService<Pelicula>,
                                         IExistService<Pelicula>
    {
        public Task<IEnumerable<Pelicula>> GetPeliculasByCategoriaID(int Id);
        public Task<IEnumerable<Pelicula>> GetPeliculasByName(string name);
    }
}
