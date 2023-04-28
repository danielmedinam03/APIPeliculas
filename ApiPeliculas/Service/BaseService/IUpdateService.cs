using ApiPeliculas.Models;

namespace ApiPeliculas.Service.IBaseService
{
    public interface IUpdateService<T> where T : class
    {
        public Task<bool> UpdateAsync(T entity);
    }
}
