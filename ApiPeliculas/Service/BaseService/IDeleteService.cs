using ApiPeliculas.Models;

namespace ApiPeliculas.Service.IBaseService
{
    public interface IDeleteService<T> where T : class
    {
        public Task<bool> DeleteAsync(int Id);
    }
}
