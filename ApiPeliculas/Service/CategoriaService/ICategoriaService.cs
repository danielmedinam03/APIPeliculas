using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos.CategoriaDTOs;
using ApiPeliculas.Service.IBaseService;

namespace ApiPeliculas.Service.CategoriaService
{
    public interface ICategoriaService : IDeleteService<Categoria>, ICreateService<Categoria>,
                                         IUpdateService<Categoria>, IGetService<Categoria>,
                                         IExistService<Categoria>
    {
    }
}
