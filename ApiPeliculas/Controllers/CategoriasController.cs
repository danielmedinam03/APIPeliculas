using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos.CategoriaDTOs;
using ApiPeliculas.Service.CategoriaService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;
        public CategoriasController(ICategoriaService categoriaService, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }
        [AllowAnonymous]    
        [HttpGet]
        //[ResponseCache(Duration = 20)] LA PETICION RESPONDE EL CACHE POR 20 SEG
        //[ResponseCache(CacheProfileName = "PorDefecto20Seg")] //PROFILE DE CACHE
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategorias()
        {
            var data = await _categoriaService.GetAllAsync();
            var dataDto = new List<CategoriaDTO>();

            foreach (var item in data)
            {
                dataDto.Add(_mapper.Map<CategoriaDTO>(item));
            }
            return Ok(dataDto);

        }

        [AllowAnonymous] //Endpoint abierto
        [HttpGet("{Id}")]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdCategorias(int Id)
        {
            var data = await _categoriaService.GetByIdAsync(Id);
            if (data is null)
            {
                return NotFound();
            }
            CategoriaDTO dataDto = (_mapper.Map<CategoriaDTO>(data));
            return Ok(dataDto);

        }
        //[Authorize(Roles ="admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategorias([FromBody] CategoriaCreateDTO Dto)
        {
            Categoria categoria = _mapper.Map<Categoria>(Dto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoria is null)
            {
                return BadRequest();
            }
            if (await _categoriaService.ExistNameAsync(categoria.Nombre))
            {
                ModelState.AddModelError("", "Ya existe una categoria con ese nombre");
                return StatusCode(404, ModelState);
            }
            var data = await _categoriaService.AddAsync(categoria);
            return Ok(data);
        }
        //[Authorize(Roles = "admin")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategorias(int Id, [FromBody] CategoriaDTO Dto)
        {
            Dto.ID = Id;
            Categoria categoria = _mapper.Map<Categoria>(Dto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoria is null)
            {
                return BadRequest();
            }
            if (!await _categoriaService.ExistAsync(categoria.ID))
            {
                ModelState.AddModelError("", "La categoría no existe");
                return StatusCode(404, ModelState);
            }
            if (await _categoriaService.ExistNameAsync(categoria.Nombre))
            {
                ModelState.AddModelError("", "Ya existe una categoria con ese nombre");
                return StatusCode(404, ModelState);
            }
            var data = await _categoriaService.UpdateAsync(categoria);
            return Ok(data);
        }
        //[Authorize(Roles = "admin")]
        [HttpDelete("{Id}")]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategorias(int Id)
        {
            Categoria cat = new Categoria();
            cat.ID=Id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _categoriaService.ExistAsync(Id))
            {
                ModelState.AddModelError("", "La categoría no existe");
                return StatusCode(404, ModelState);
            }
            
            var data = await _categoriaService.DeleteAsync(Id);
            return Ok(data);
        }

    }
}
