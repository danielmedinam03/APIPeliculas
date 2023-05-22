using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Utils;

namespace PeliculasWeb.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public CategoriasController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public IActionResult Index()
        {
            return View(new Categoria() { });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategorias()
        {
            return Json(new {data = await _categoriaRepository.GetAllAsync(CT.RutaCategoriasApi) });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid) 
            {
                await _categoriaRepository.AddAsync(CT.RutaCategoriasApi,categoria);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            Categoria categoria = new();
            if (Id == null) 
            {
                return NotFound();
            }
            categoria = await _categoriaRepository.GetByIdAsync(CT.RutaCategoriasApi,Id.GetValueOrDefault());
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoriaRepository.UpdateAsync(CT.RutaCategoriasApi + categoria.Id, categoria);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var status = await _categoriaRepository.DeleteAsync(CT.RutaCategoriasApi,Id);

            if (status is true)
                return Json(new { succes = true,message = "Borrado Correctamente"});

            return Json(new { succes = true, message = "Error al borrar" });
        }

    }
}
