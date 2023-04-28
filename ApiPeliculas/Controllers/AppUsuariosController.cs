using ApiPeliculas.Models.Dtos.PeliculaDTOs;
using ApiPeliculas.Models.Dtos.UsuarioDTOs;
using ApiPeliculas.Models;
using ApiPeliculas.Service.AppUsuarioService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AutoMapper;

namespace ApiPeliculas.Controllers
{
    [Route("api/NetIdentity/[controller]")]
    [ApiController]
    public class AppUsuariosController : ControllerBase
    {
        private readonly IAppUsuarioService _service;
        protected RespuestaAPI _respuestaApi;
        private readonly IMapper _mapper;

        public AppUsuariosController(IAppUsuarioService appUsuarioService,IMapper mapper)
        {
            _service = appUsuarioService;
            this._respuestaApi = new();
            _mapper = mapper;

        }

        [AllowAnonymous]
        [HttpPost("Registro")]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDTO Dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (Dto is null)
            {
                return BadRequest();
            }
            if (await _service.IsUniqueUser(Dto.NombreUsuario))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El nombre de usuario ya existe");
                return BadRequest(_respuestaApi);
            }
            var data = await _service.Registro(Dto);
            if (data is null)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error en el registro");
                return BadRequest(_respuestaApi);
            }
            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.Result = data;
            return Ok(_respuestaApi);
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO Dto)
        {

            var data = await _service.Login(Dto);

            if (data.Usuario is null || data.Token is null)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El nombre de usuario o contraseña son incorrectos");
                return BadRequest(_respuestaApi);
            }

            if (Dto is null)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("La información suministrada es incorrecta");
                return BadRequest(_respuestaApi);
            }
            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.Result = data;
            return Ok(_respuestaApi);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _service.GetAllAsync();
            var listDTO = new List<UsuarioDTO>();
            foreach (var dto in data)
            {
                listDTO.Add(_mapper.Map<UsuarioDTO>(dto));
            }
            return Ok(listDTO);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("{Id}")]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(string Id)
        {
            var data = await _service.GetByIdAsync(Id);
            if (data is null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<UsuarioDTO>(data);
            return Ok(dto);
        }
    }
}
