using ItAcademyDell.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace ItAcademyDell.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Orcamento()
        {
            return View(new HomeModel());
        }

        [HttpPost]
        public IActionResult Orcamento(HomeModel model)
        {
            var caminhaoSelecionado = TransporteData.ModelosDeTransporte().First(x => x.Id == model.ModeloDeTransporte);

            model.Distancia = TransporteData.BuscarDistanciaEntreCidades(model.CidadeOrigem, model.CidadeDestino);
            model.CustoTotal = caminhaoSelecionado.PrecoPorKm * model.Distancia;
            model.DescricaoTransporte = caminhaoSelecionado.Nome;
            model.ValoresCalculados = true;
           
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}