using PeliculasWeb.Models;

namespace PeliculasWeb.Repository.IRepository
{
    public interface IAccountRepository : IBaseRepository<UsuarioM>
    {
        public Task<UsuarioM> LoginAsync(string url, UsuarioM itemCrear);
        public Task<bool> RegisterAsync(string url, UsuarioM itemCrear);
    }
}
