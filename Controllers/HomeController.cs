using Microsoft.AspNetCore.Mvc;
using SistemaBiblioteca.Models;
using System.Diagnostics;

namespace SistemaBiblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Página de inicio con saludo y botón de búsqueda
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Página de privacidad (opcional)
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Página de error
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}