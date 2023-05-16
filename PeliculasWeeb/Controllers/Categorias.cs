using Microsoft.AspNetCore.Mvc;

namespace PeliculasWeb.Controllers
{
    public class Categorias : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
