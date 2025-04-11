using Microsoft.AspNetCore.Mvc;
using InmobiliariaLopez.Repositories;
using InmobiliariaLopez.Models;

namespace InmobiliariaLopez.Controllers
{
    public class ContratosController : Controller
    {
        private readonly IRepositorioContrato _repositorioContrato;

        public ContratosController(IRepositorioContrato repositorioContrato)
        {
            _repositorioContrato = repositorioContrato;
        }

        // GET: Contratos
        public IActionResult Index()
        {
            var contratos = _repositorioContrato.Index();
            return View(contratos);
        }

        // GET: Contratos/Details/5
        public IActionResult Details(int id)
        {
            var contrato = _repositorioContrato.Details(id);

            if (contrato == null)
            {
              return NotFound();
            }

            return View(contrato);
        }
    }
}
