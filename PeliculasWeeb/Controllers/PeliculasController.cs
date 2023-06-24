using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeliculasWeb.Domain.DTOs;
using PeliculasWeb.Models;
using PeliculasWeb.Models.ViewModels;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Utils;
using System.IO;

namespace PeliculasWeb.Controllers
{
    public class PeliculasController : Controller
    {

        private readonly IPeliculaRepository _repository;
        private readonly ICategoriaRepository _repositoryCategoria;
        public PeliculasController(IPeliculaRepository repository, ICategoriaRepository repositoryCategoria)
        {
            _repository = repository;
            _repositoryCategoria = repositoryCategoria;
        }

        [HttpGet]
        public IActionResult Index() 
        {
            return View(new Pelicula() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPeliculas()
        {
            return Json(new {data = await _repository.GetAllAsync(CT.RutaPeliculasApi) });
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Categoria> npList = await _repositoryCategoria.GetAllAsync(CT.RutaCategoriasApi);
            PeliculasVM objVM = new()
            {
                ListaCategorias = npList.Select(x => new SelectListItem{
                   Text = x.Nombre,
                   Value = x.Id.ToString()
                }),

                Pelicula = new Pelicula()

            };

            return View(objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PeliculaDTO pelicula)
        {
            var memoryStream = new MemoryStream();
            
            pelicula.RutaImagen.CopyTo(memoryStream);
            var imagenPelicula = memoryStream.ToArray();

            Pelicula peliculaEntity = new()
            {
                Id = pelicula.Id,
                CategoriaId = pelicula.CategoriaId,
                Clasificacion = pelicula.Clasificacion,
                Descripcion = pelicula.Descripcion,
                Duracion = pelicula.Duracion,
                FechaCreacion = pelicula.FechaCreacion,
                Nombre = pelicula.Nombre,
                RutaImagen = Convert.ToBase64String(imagenPelicula),
            };

            IEnumerable<Categoria> npList = await _repositoryCategoria.GetAllAsync(CT.RutaCategoriasApi);
            PeliculasVM objVM = new()
            {
                ListaCategorias = npList.Select(x => new SelectListItem
                {
                    Text = x.Nombre,
                    Value = x.Id.ToString()
                }),

                Pelicula = new Pelicula()

            };

            
            var data = await _repository.AddAsync(CT.RutaPeliculasApi, peliculaEntity, HttpContext.Session.GetString("JWToken"));
            if (data is false)
            {
                TempData["alertDanger"] = "Usuario no autorizado para crear una pelicula";
            }
            if (data is true)
            {
                TempData["alertSuccess"] = "Pelicula creada con éxito !";
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            IEnumerable<Categoria> npList = await _repositoryCategoria.GetAllAsync(CT.RutaCategoriasApi);
            PeliculasEditVM objVM = new()
            {
                ListaCategorias = npList.Select(x => new SelectListItem
                {
                    Text = x.Nombre,
                    Value = x.Id.ToString()
                }),

                Pelicula = new PeliculasEditDTO()

            };

            if (Id==null)
            {
                return NotFound();
            }
            //Para mostrar los datos
            Pelicula peliculaResponse = new();
            peliculaResponse = await _repository.GetByIdAsync(CT.RutaPeliculasApi, Id.GetValueOrDefault());

            //var peliculaArrayBytes = Convert.FromBase64String(peliculaResponse.RutaImagen);
            //string outputPath = "C:\\Users\\KUBIT\\Desktop\\imagenPrueba.jpg";

            //;

            objVM.Pelicula = new PeliculasEditDTO()
            {
                Id = peliculaResponse.Id,
                CategoriaId = peliculaResponse.CategoriaId,
                Clasificacion = peliculaResponse.Clasificacion,
                Descripcion = peliculaResponse.Descripcion,
                Duracion = peliculaResponse.Duracion,
                FechaCreacion = peliculaResponse.FechaCreacion,
                Nombre = peliculaResponse.Nombre,
                RutaImagen = peliculaResponse.RutaImagen
            };

            if (objVM.Pelicula == null)
            {
                return NotFound();
            }

            return View(objVM);

        }
        [HttpPost]
        public async Task<IActionResult> Update(PeliculaDTO pelicula)
        {
            byte[] imagenPelicula = { };

            if (pelicula.RutaImagen is not null)
            {
                var memoryStream = new MemoryStream();

                pelicula.RutaImagen.CopyTo(memoryStream);
                imagenPelicula = memoryStream.ToArray();
            }
            

            Pelicula peliculaEntity = new()
            {
                Id = pelicula.Id,
                CategoriaId = pelicula.CategoriaId,
                Clasificacion = pelicula.Clasificacion,
                Descripcion = pelicula.Descripcion,
                Duracion = pelicula.Duracion,
                FechaCreacion = pelicula.FechaCreacion,
                Nombre = pelicula.Nombre,
                RutaImagen = Convert.ToBase64String(imagenPelicula),
            };

            IEnumerable<Categoria> npList = await _repositoryCategoria.GetAllAsync(CT.RutaCategoriasApi);
            PeliculasVM objVM = new()
            {
                ListaCategorias = npList.Select(x => new SelectListItem
                {
                    Text = x.Nombre,
                    Value = x.Id.ToString()
                }),

                Pelicula = peliculaEntity

            };

            var data = await _repository.UpdateAsync(CT.RutaPeliculasApi + pelicula.Id, peliculaEntity, HttpContext.Session.GetString("JWToken"));
            if (data is false)
            {
                TempData["alertDanger"] = "Usuario no autorizado para editar una pelicula";
            }
            else if (data is true)
            {
                TempData["alertSuccess"] = "Pelicula editada con exito";
            }

            return RedirectToAction(nameof(Index));

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var status = await _repository.DeleteAsync(CT.RutaPeliculasApi, Id, HttpContext.Session.GetString("JWToken"));

            if (status is true)
                return Json(new { succes = true, message = "Borrado Correctamente" });

            return Json(new { succes = true, message = "Error al borrar" });
        }
        [HttpGet]
        public async Task<IActionResult> GetPeliculasEnCategoria(int id)
        {
            return Json(new { data = await _repository.GetPeliculasEnCategoriasAsync(CT.RutaPeliculasEnCategoriasApi, id) });
        }
    }
}
