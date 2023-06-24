using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos.UsuarioDTOs;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiPeliculas.Service.UsuarioService
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private string claveSecreta;
        public UsuarioService(IUsuarioRepository usuarioRepository, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            
        }

        public async Task<bool> ExistAsync(int id)
        {
            var data = await _usuarioRepository.ExistGenericAsync(x => x.Id==id);
            return data;
        }

        public Task<bool> ExistNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var data = await _usuarioRepository.GenericGetAllAsync();
            return data;
        }

        public async Task<Usuario> GetByIdAsync(int Id)
        {
            var data = await _usuarioRepository.GenericGetAsync(x => x, x => x.Id==Id);
            return data;
        }

        public async Task<bool> IsUniqueUser(string user)
        {
            var data = await _usuarioRepository.ExistGenericAsync(x => x.NombreUsuario.Trim().Equals(user.Trim()));
            return data;
        }

        public async Task<UsuarioRepuestaLoginDTO> Login(UsuarioLoginDTO usuarioDto)
        {
            var passEncriptada = obtenermd5(usuarioDto.Password);
            var user = await _usuarioRepository.GenericGetAsync(x => x,
                x => x.NombreUsuario.ToLower().Trim() == usuarioDto.NombreUsuario.ToLower().Trim()
                && x.Password == passEncriptada);

            //Validamos si el usuario no existe con la contraseña correcta
            if (user == null)
            {
                return new UsuarioRepuestaLoginDTO()
                {
                    Token = "",
                    Usuario = null
                };
            }
            //Aquí si existe el usuario
            var manToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role, user.Rol)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new (new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manToken.CreateToken(tokenDescriptor);

            UsuarioRepuestaLoginDTO usuarioLoginRespuestaDTO = new()
            {
                Token = manToken.WriteToken(token),
                Usuario = user
            };
            return usuarioLoginRespuestaDTO;
        }

        //public async Task<Usuario> Registro(UsuarioRegistroDTO usuarioDto)
        //{
        //    var passEncriptada = obtenermd5(usuarioDto.Password);
        //    Usuario usuario = new()
        //    {
        //        NombreUsuario = usuarioDto.NombreUsuario,
        //        Password = passEncriptada,
        //        Nombre = usuarioDto.Nombre,
        //        Rol = usuarioDto.Rol
        //    };

        //    await _usuarioRepository.AddAsync(usuario);
        //    usuario.Password = passEncriptada;
        //    return usuario;
        //}

        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
            {
                resp += data[i].ToString("x2").ToLower();
            }
            return resp;

        }
        /// <summary>
        /// Metodo no se usa
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Usuario> GetByIdAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> Registro(UsuarioRegistroDTO usuarioDto)
        {
            throw new NotImplementedException();
        }
    }
}
