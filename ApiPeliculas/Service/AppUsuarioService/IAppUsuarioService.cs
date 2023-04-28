using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos.UsuarioDTOs;
using ApiPeliculas.Service.IBaseService;

namespace ApiPeliculas.Service.AppUsuarioService
{
    public interface IAppUsuarioService : IGetService<AppUsuario>
    {
        public Task<AppUsuarioLoginRespuestaDTO> Login(UsuarioLoginDTO usuarioDto);
        public Task<UsuarioDatoDTO> Registro(UsuarioRegistroDTO usuarioDto);
        public Task<bool> IsUniqueUser(string user);
    }
}
