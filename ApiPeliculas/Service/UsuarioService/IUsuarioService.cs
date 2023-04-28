using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos.UsuarioDTOs;
using ApiPeliculas.Service.IBaseService;

namespace ApiPeliculas.Service.UsuarioService
{
    public interface IUsuarioService : IGetService<Usuario>
    {
        public Task<UsuarioRepuestaLoginDTO> Login(UsuarioLoginDTO usuarioDto);
        public Task<Usuario> Registro(UsuarioRegistroDTO usuarioDto);
        public Task<bool> IsUniqueUser(string user);
    }
}
