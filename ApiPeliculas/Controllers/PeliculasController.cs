using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos.CategoriaDTOs;
using ApiPeliculas.Models.Dtos.PeliculaDTOs;
using ApiPeliculas.Service.PeliculaService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaService _peliculaService;
        private readonly IMapper _mapper;
        public PeliculasController(IPeliculaService peliculaService, IMapper mapper)
        {
            _peliculaService = peliculaService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPeliculas()
        {
            var data = await _peliculaService.GetAllAsync();
            var dataDto = new List<PeliculaDTO>();

            foreach (var item in data)
            {
                dataDto.Add(_mapper.Map<PeliculaDTO>(item));
            }
            return Ok(dataDto);

        }
        [AllowAnonymous]
        [HttpGet("{Id}")]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdPeliculas(int Id)
        {
            var data = await _peliculaService.GetByIdAsync(Id);
            if (data is null)
            {
                return NotFound();
            }
            PeliculaDTO dataDto = (_mapper.Map<PeliculaDTO>(data));
            return Ok(dataDto);

        }
        [AllowAnonymous]
        [HttpGet("Categoria/{Id}")]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdCategoria(int Id)
        {
            var data = await _peliculaService.GetPeliculasByCategoriaID(Id);
            var dataDto = new List<PeliculaDTO>();

            foreach (var item in data)
            {
                dataDto.Add(_mapper.Map<PeliculaDTO>(item));
            }
            return Ok(dataDto);

        }
        [AllowAnonymous]
        [HttpGet("Nombre/{nombre}")]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByNombre(string nombre)
        {
            var data = await _peliculaService.GetPeliculasByName(nombre);
            var dataDto = new List<PeliculaDTO>();

            foreach (var item in data)
            {
                dataDto.Add(_mapper.Map<PeliculaDTO>(item));
            }
            return Ok(dataDto);

        }
        //[Authorize(Roles ="admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePeliculas([FromBody] PeliculaDTO Dto)
        {
            //var memoryStream = new MemoryStream();

            //Dto.RutaImagen.CopyTo(memoryStream);


            Pelicula pelicula = new()
            {
                CategoriaId=Dto.CategoriaId,
                Clasificacion=Dto.Clasificacion,
                Descripcion=Dto.Descripcion,
                Duracion=Dto.Duracion,
                FechaCreacion=Dto.FechaCreacion,
                Id=Dto.Id,
                Nombre=Dto.Nombre,
                //RutaImagen = memoryStream.ToArray()
                RutaImagen = Dto.RutaImagen
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (pelicula is null)
            {
                return BadRequest();
            }
            if (await _peliculaService.ExistNameAsync(pelicula.Nombre))
            {
                ModelState.AddModelError("", "Ya existe una pelicula con ese nombre");
                return StatusCode(404, ModelState);
            }
            var data = await _peliculaService.AddAsync(pelicula);
            return Ok(data);
        }
        //[Authorize(Roles = "admin")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePelicula(int Id, [FromBody] PeliculaDTO Dto)
        {
            Dto.Id = Id;
            Pelicula pelicula = _mapper.Map<Pelicula>(Dto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (pelicula is null)
            {
                return BadRequest();
            }
            if (!await _peliculaService.ExistAsync(pelicula.Id))
            {
                ModelState.AddModelError("", "La categoría no existe");
                return StatusCode(404, ModelState);
            }
            if (await _peliculaService.ExistNameAsync(pelicula.Nombre))
            {
                ModelState.AddModelError("", "Ya existe una categoria con ese nombre");
                return StatusCode(404, ModelState);
            }
            var data = await _peliculaService.UpdateAsync(pelicula);
            return Ok(data);
        }
        //[Authorize(Roles = "admin")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePelicula(int Id)
        {
            Pelicula cat = new Pelicula();
            cat.Id = Id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _peliculaService.ExistAsync(Id))
            {
                ModelState.AddModelError("", "La categoría no existe");
                return StatusCode(404, ModelState);
            }

            var data = await _peliculaService.DeleteAsync(Id);
            return Ok(data);
        }


    }
}
