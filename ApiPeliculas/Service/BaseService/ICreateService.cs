using ApiPeliculas.Models;

namespace ApiPeliculas.Service.IBaseService
{
    public interface ICreateService<T> where T : class
    {
        public Task<int> AddAsync(T entity);
    }
}
