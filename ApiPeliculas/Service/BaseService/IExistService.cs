using ApiPeliculas.Models;

namespace ApiPeliculas.Service.IBaseService
{
    public interface IExistService<T> where T : class
    {
        public Task<bool> ExistNameAsync(string name);
        public Task<bool> ExistAsync(int id);
    }
}
