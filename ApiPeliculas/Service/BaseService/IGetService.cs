using ApiPeliculas.Models;

namespace ApiPeliculas.Service.IBaseService
{
    public interface IGetService<T> where T : class
    {
        public Task<T> GetByIdAsync(int Id);
        public Task<T> GetByIdAsync(string Id);
        public Task<IEnumerable<T>> GetAllAsync();
    }
}
