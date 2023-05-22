using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
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
        public HomeController(ILogger<HomeController> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
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
        public IActionResult Registro()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}