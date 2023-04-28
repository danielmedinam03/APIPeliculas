using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos.UsuarioDTOs;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiPeliculas.Service.AppUsuarioService
{
    public class AppUsuarioService : IAppUsuarioService
    {
        private readonly IAppUsuarioRepository _appUsuarioRepository;

        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private string claveSecreta;

        public AppUsuarioService(IAppUsuarioRepository appUsuarioRepository, UserManager<AppUsuario> userManager, 
            RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration config)
        {
            _appUsuarioRepository = appUsuarioRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
        }
        public async Task<IEnumerable<AppUsuario>> GetAllAsync()
        {
            var data = await _appUsuarioRepository.GenericGetAllAsync(null,null,x => x.OrderBy(c => c.UserName));
            return data;
        }

        public async Task<AppUsuario> GetByIdAsync(string Id)
        {
            var data = await _appUsuarioRepository.GenericGetAsync(x => x, x => x.Id==Id);
            return data;
        }


        public async Task<bool> IsUniqueUser(string user)
        {
            var data = await _appUsuarioRepository.ExistGenericAsync(x => x.UserName.Trim().Equals(user.Trim()));
            return data;
        }

        public async Task<AppUsuarioLoginRespuestaDTO> Login(UsuarioLoginDTO usuarioDto)
        {
            var usuario = await _appUsuarioRepository.GenericGetAsync(x =>x,
                x => x.UserName.ToLower().Trim() == usuarioDto.NombreUsuario.ToLower().Trim());

            bool isValid = await _userManager.CheckPasswordAsync(usuario, usuarioDto.Password);
            if (usuario is null || isValid is false) 
            {
                return new AppUsuarioLoginRespuestaDTO()
                {
                    Token = "",
                    Usuario = null
                };
            }
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var roles = await _userManager.GetRolesAsync(usuario);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = manejadorToken.CreateToken(tokenDescriptor);
            AppUsuarioLoginRespuestaDTO usuarioRepuestaLoginDto = new AppUsuarioLoginRespuestaDTO()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatoDTO>(usuario)
            };
            return usuarioRepuestaLoginDto;
        }

        public async Task<UsuarioDatoDTO> Registro(UsuarioRegistroDTO usuarioDto)
        {
            AppUsuario usuario = new()
            {
                UserName = usuarioDto.NombreUsuario,
                Email = usuarioDto.NombreUsuario,
                NormalizedEmail = usuarioDto.NombreUsuario.ToUpper(),
                Nombre = usuarioDto.Nombre
            };

            var result = await _userManager.CreateAsync(usuario, usuarioDto.Password);
            if (result.Succeeded)
            {
                //Solo la primera vez y es para crear los roles
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }
                //A partir de la primera vez, cambiar el nombre del rol
                await _userManager.AddToRoleAsync(usuario, "admin");
                var usuarioRetornado = await _appUsuarioRepository.GenericGetAsync(x => x, x => x.UserName == usuarioDto.NombreUsuario);
                //  Opcion 1
                //return new UsuarioDatoDTO()
                //{
                //    Id = usuarioRetornado.Id,
                //    UserName = usuarioRetornado.UserName,
                //    Nombre = usuarioRetornado.Nombre
                //};

                return _mapper.Map<UsuarioDatoDTO>(usuarioRetornado);
            }
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.ToString());
            }
            return new UsuarioDatoDTO();
        }





        /// <summary>
        /// Metodo no usado
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<AppUsuario> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

    }
}
