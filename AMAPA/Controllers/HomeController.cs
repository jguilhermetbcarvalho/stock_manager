using AMAPA.Models;
using AMAPA.Repository;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AMAPA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Usuarios _user;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var errorModel = new ErrorViewModel
            {
                RequestId = requestId,
                ErrorMessage = exceptionHandlerPathFeature?.Error.Message  // Captura a mensagem de erro
            };

            // Logue o erro incluindo o RequestId e a mensagem de erro
            _logger.LogError(exceptionHandlerPathFeature?.Error, "An error occurred. Request ID: {RequestId}", requestId);

            return View(errorModel);
        }

    }
}
