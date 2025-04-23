using InmobiliariaLopez.Models;
using InmobiliariaLopez.Models.Dtos; // Para incluir el DTO
using InmobiliariaLopez.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaLopez.Controllers
{
    public class PagosController : Controller
    {
        private readonly IRepositorioPago _pagoRepository;

        public PagosController(IRepositorioPago pagoRepository)
        {
            _pagoRepository = pagoRepository;
        }

        // GET: Pago
        public IActionResult Index()
        {
            var pagos = _pagoRepository.Index();
            return View(pagos);
        }

        // GET: Pago/Details/5
        public IActionResult Details(int id)
        {
            var pago = _pagoRepository.Details(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        // GET: Pago/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pago pago)
        {
            if (ModelState.IsValid)
            {
                pago.IdUsuarioCrea = HttpContext.Session.GetInt32("IdUsuario");
                pago.FechaCreacion = DateTime.Now;
                _pagoRepository.Create(pago);
                return RedirectToAction(nameof(Index));
            }
            return View(pago);
        }

        // GET: Pago/Edit/5
        public IActionResult Edit(int id)
        {
            var pago = _pagoRepository.Details(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Pago pago)
        {
            if (id != pago.IdPago)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _pagoRepository.Edit(pago);
                return RedirectToAction(nameof(Index));
            }
            return View(pago);
        }

        // GET: Pago/Delete/5
        public IActionResult Delete(int id)
        {
            var pago = _pagoRepository.Details(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _pagoRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Pago/PagosPorContrato/5
        public IActionResult PagosPorContrato(int contratoId)
        {
            var pagos = _pagoRepository.ObtenerPagosPorContrato(contratoId);
            return View(pagos);
        }

        // GET: Pago/Anular/5
        public IActionResult Anular(int id)
        {
            var pago = _pagoRepository.Details(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        // POST: Pago/Anular/5
        [HttpPost]
        public IActionResult Anular([FromBody] AnularPagoDTO anularPagoDTO)
        {
            try
            {
                // Log para depuración
                Console.WriteLine(
                    $"DTO recibido: IdPago={anularPagoDTO.IdPago}, MotivoAnulacion={anularPagoDTO.MotivoAnulacion}, FechaAnulacion={anularPagoDTO.FechaAnulacion}, IdUsuarioAnula={anularPagoDTO.IdUsuarioAnula}"
                );

                // Buscar el pago en la base de datos usando el IdPago del DTO
                var pago = _pagoRepository.Details(anularPagoDTO.IdPago);

                if (pago == null)
                {
                    return Json(new { success = false, message = "Pago no encontrado." });
                }

                var resultado = _pagoRepository.AnularPago(
                    anularPagoDTO.IdPago, // ID del pago
                    anularPagoDTO.IdUsuarioAnula, // ID del usuario que está anulando
                    anularPagoDTO.MotivoAnulacion // Motivo de la anulación
                );

                if (resultado > 0)
                {
                    return Json(new { success = true, redirectUrl = Url.Action("Index") });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo anular el pago." });
                }
            }
            catch (Exception ex)
            {
                // Log de error para depuración
                Console.WriteLine("Error al procesar el DTO: " + ex.Message);
                return Json(
                    new { success = false, message = "Hubo un error al procesar la solicitud." }
                );
            }
        }
    }
}
