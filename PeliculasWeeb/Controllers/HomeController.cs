using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Models.ViewModels;
using PeliculasWeb.Repository.IRepository;
using PeliculasWeb.Utils;
using System.Diagnostics;
using System.Security.Claims;

namespace PeliculasWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepository _accountRepository;

        private readonly IPeliculaRepository _repositoryPelicula;
        private readonly ICategoriaRepository _repositoryCategoria;
        public HomeController(ILogger<HomeController> logger, IAccountRepository accountRepository,
            IPeliculaRepository repositoryPelicula, ICategoriaRepository categoriaRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _repositoryCategoria = categoriaRepository;
            _repositoryPelicula = repositoryPelicula;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM listaPleiculasCategoria = new()
            {
                ListaCategorias = await _repositoryCategoria.GetAllAsync(CT.RutaCategoriasApi),
                ListaPeliculas = await _repositoryPelicula.GetAllAsync(CT.RutaPeliculasApi)
            };
            return View(listaPleiculasCategoria);
        }

        public async Task<IActionResult> IndexCategoria(int id)
        {
            var peliculaEnCategoria = await _repositoryPelicula.GetPeliculasEnCategoriasAsync(CT.RutaPeliculasEnCategoriasApi + id,id);
            return View(peliculaEnCategoria);
        }
        public async Task<IActionResult> IndexBusqueda(string nombre)
        {
            var peliculasEncontradas = await _repositoryPelicula.Buscar(CT.RutaPeliculasApiBusqueda,nombre);
            return View(peliculasEncontradas);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View(new UsuarioM());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UsuarioM usuario)
        {
            if (ModelState.IsValid)
            {
                UsuarioM objUser = await _accountRepository.LoginAsync(CT.RutaUsuariosApi + "Login", usuario);
                if (objUser.token is null)
                {
                    TempData["alert"] = "Los datos son incorrectos";
                    return View();
                }

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, objUser.userName));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("JWToken", objUser.token);
                TempData["alert"] = $"Bienvenido/a  {objUser.userName}!";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                return View(new UsuarioM());
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken","");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(UsuarioM usuario)
        {
            bool result = await _accountRepository.RegisterAsync(CT.RutaUsuariosApi + "Registro",usuario);
            if (!result)
            {
                return View();
            }
            TempData["alert"] = "Registro correcto !";
            return RedirectToAction(nameof(Login));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}