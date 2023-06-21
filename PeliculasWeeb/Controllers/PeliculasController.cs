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
            

            Pelicula peliculaEntity = new()
            {
                Id = pelicula.Id,
                CategoriaId = pelicula.CategoriaId,
                Clasificacion = pelicula.Clasificacion,
                Descripcion = pelicula.Descripcion,
                Duracion = pelicula.Duracion,
                FechaCreacion = pelicula.FechaCreacion,
                Nombre = pelicula.Nombre,
                RutaImagen = memoryStream.ToArray()
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

            if (ModelState.IsValid)
            {
                //var files = HttpContext.Request.Form.Files;
                //if (files.Count > 0)
                //{
                //    byte[] p1 = null;
                //    using (var fs1 = files[0].OpenReadStream())
                //    {
                //        using (var ms1 = new MemoryStream())
                //        {
                //            fs1.CopyTo(ms1);
                //            p1 = ms1.ToArray();
                //        }
                //    }
                //    pelicula.RutaImagen = p1;
                //}
                //else
                //{
                //    return View(objVM);
                //}

                await _repository.AddAsync(CT.RutaPeliculasApi, peliculaEntity, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));

            }
            return View(objVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
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

            if (Id==null)
            {
                return NotFound();
            }
            //Para mostrar los datos

            objVM.Pelicula = await _repository.GetByIdAsync(CT.RutaPeliculasApi, Id.GetValueOrDefault());
            if (objVM.Pelicula == null)
            {
                return NotFound();
            }

            return View(objVM);

        }
        [HttpPost]
        public async Task<IActionResult> Update(Pelicula pelicula)
        {
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

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    pelicula.RutaImagen = p1;
                }
                else
                {
                    var peliculaDB = await _repository.GetByIdAsync(CT.RutaPeliculasApi, pelicula.Id);
                    pelicula.RutaImagen = peliculaDB.RutaImagen;
                }

                await _repository.UpdateAsync(CT.RutaPeliculasApi + pelicula.Id, pelicula, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));

            }
            return View();
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
