using InmobiliariaLopez.Models;
using InmobiliariaLopez.Models.Dtos; // Para incluir el DTO
using InmobiliariaLopez.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmobiliariaLopez.Controllers
{
    public class PagosController : Controller
    {
        private readonly IRepositorioPago _pagoRepository;
        private readonly IRepositorioContrato _contratoRepository;

        public PagosController(
            IRepositorioPago pagoRepository,
            IRepositorioContrato contratoRepository
        )
        {
            _pagoRepository = pagoRepository;
            _contratoRepository = contratoRepository;
        }

        // GET: Pago
        public IActionResult Index(int pagina = 1)
        {
            var pagos = _pagoRepository.Index(pagina);
            int cantidadTotal = _pagoRepository.ObtenerTotal();
            int registrosPorPagina = 10;
            int totalPaginas = (int)Math.Ceiling((double)cantidadTotal / registrosPorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

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
            // Obtener contratos
            var contratos = _contratoRepository.Index();

            var contratosSelectList = contratos
                .Select(c => new SelectListItem
                {
                    Value = c.IdContrato.ToString(),
                    Text = $"{c.DireccionInmueble} - {c.NombreInquilino}",
                })
                .ToList();

            ViewBag.Contratos = contratosSelectList;

            // Obtener multas sin pagar
            var multas = _pagoRepository.ObtenerMultasSinPagar();

            var multasSelectList = multas
                .Select(m => new SelectListItem
                {
                    Value = m.IdMulta.ToString(),
                    Text = $"{m.IdMulta} - {m.Motivo}|{m.Monto}",
                })
                .ToList();

            ViewBag.MultasSinPagar = multasSelectList;

            return View();
        }

        // GET: Pago/ObtenerMultasPorContrato
        [HttpGet]
        public IActionResult ObtenerMultasPorContrato(int contratoId)
        {
            var multas = _pagoRepository
                .ObtenerMultasSinPagar()
                .Where(m => m.IdContrato == contratoId)
                .Select(m => new
                {
                    m.IdMulta,
                    m.Monto,
                    m.Motivo,
                })
                .ToList();

            return Json(multas);
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pago pago)
        {
            if (!ModelState.IsValid)
            {
                // Si es una solicitud AJAX, devolvemos errores en JSON
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errores = ModelState
                        .Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return Json(new { success = false, message = string.Join("<br>", errores) });
                }

                return View(pago);
            }

            var idUsuarioClaim = User.FindFirst("IdUsuario");
            if (idUsuarioClaim == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Usuario no autenticado." });
                }

                ModelState.AddModelError("", "Usuario no autenticado.");
                return View(pago);
            }

            pago.IdUsuarioCrea = int.Parse(idUsuarioClaim.Value);
            pago.FechaCreacion = DateTime.Now;

            var ultimoNumeroPago = _pagoRepository.ObtenerUltimoNumeroPago(pago.IdContrato);
            pago.NumeroPagoContrato = (ultimoNumeroPago ?? 0) + 1;

            var idPagoCreado = _pagoRepository.Create(pago);

            // Si es una solicitud AJAX, devolvemos la URL de redirección
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(
                    new
                    {
                        success = true,
                        redirectUrl = Url.Action("Details", new { id = idPagoCreado }),
                    }
                );
            }

            // Si no es AJAX, redirigimos a la vista de detalles
            return RedirectToAction("Details", new { id = idPagoCreado });
        }

        // GET: Pago/Edit/5
        public IActionResult Edit(int id)
        {
            // Obtener el pago que se va a editar
            var pago = _pagoRepository.Details(id);
            if (pago == null)
            {
                return NotFound();
            }

            // Obtener contratos
            var contratos = _contratoRepository.Index();
            var contratosSelectList = contratos
                .Select(c => new SelectListItem
                {
                    Value = c.IdContrato.ToString(),
                    Text = $"{c.DireccionInmueble} - {c.NombreInquilino}",
                })
                .ToList();
            ViewBag.Contratos = contratosSelectList;

            // Obtener multas sin pagar
            var multas = _pagoRepository.ObtenerMultasSinPagar();
            var multasSelectList = multas
                .Select(m => new SelectListItem
                {
                    Value = m.IdMulta.ToString(),
                    Text = $"{m.IdMulta} - {m.Motivo}|{m.Monto}",
                })
                .ToList();
            ViewBag.MultasSinPagar = multasSelectList;

            // Devolver la vista con el pago y las listas
            return View(pago);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Pago pago)
        {
            if (id != pago.IdPago)
                return NotFound();

            if (ModelState.IsValid)
            {
                var pagoExistente = _pagoRepository.Details(id);
                if (pagoExistente == null)
                    return NotFound();

                // Solo actualizamos los campos editables
                pagoExistente.FechaPago = pago.FechaPago;
                pagoExistente.Estado = pago.Estado;
                pagoExistente.Detalle = pago.Detalle;

                _pagoRepository.Edit(pagoExistente);
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
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _pagoRepository.Delete(id);
                TempData["SuccessMessage"] = "El pago fue eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log opcional
                // _logger.LogError(ex, $"Error al eliminar el pago con ID {id}");

                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el pago.";
                return RedirectToAction(nameof(Index));
            }
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

        // POST: Pago/Anular
        [HttpPost]
        public IActionResult Anular([FromBody] AnularPagoDTO anularPagoDTO)
        {
            try
            {
                // Validación del motivo
                if (string.IsNullOrWhiteSpace(anularPagoDTO.MotivoAnulacion))
                {
                    return Json(
                        new
                        {
                            success = false,
                            message = "Debe proporcionar un motivo de anulación.",
                        }
                    );
                }

                // Obtener usuario autenticado desde claims
                var idUsuarioClaim = User.FindFirst("IdUsuario");
                if (idUsuarioClaim == null)
                {
                    return Json(new { success = false, message = "Usuario no autenticado." });
                }

                int idUsuarioAnula = int.Parse(idUsuarioClaim.Value);

                // Buscar el pago
                var pago = _pagoRepository.Details(anularPagoDTO.IdPago);
                if (pago == null)
                {
                    return Json(new { success = false, message = "Pago no encontrado." });
                }

                // Ejecutar anulación con ID del usuario logueado
                var resultado = _pagoRepository.AnularPago(
                    anularPagoDTO.IdPago,
                    idUsuarioAnula, // tomado del claim
                    anularPagoDTO.MotivoAnulacion
                );

                if (resultado > 0)
                {
                    return Json(
                        new
                        {
                            success = true,
                            redirectUrl = Url.Action("Details", new { id = anularPagoDTO.IdPago }),
                        }
                    );
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo anular el pago." });
                }
            }
            catch (Exception ex)
            {
                return Json(
                    new { success = false, message = "Hubo un error al procesar la solicitud." }
                );
            }
        }

        // GET: Pago/ObtenerImporteContrato
        [HttpGet]
        public IActionResult ObtenerImporteContrato(int id)
        {
            try
            {
                var contrato = _contratoRepository.Details(id);
                if (contrato != null)
                {
                    return Json(new { success = true, monto = contrato.MontoMensual });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
    }
}
