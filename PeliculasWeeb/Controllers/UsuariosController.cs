using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Utils;

namespace PeliculasWeb.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuariosController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public IActionResult Index()
        {
            return View(new UsuarioU() { });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            return Json(new {data = await _usuarioRepository.GetAllAsync(CT.RutaUsuariosApi) });
        }

    }
}
