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
                    Text = $"{m.IdMulta} - {m.Motivo}|{m.Monto}" // ⚠️ "|" separa el texto visible del importe
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
            if (ModelState.IsValid)
            {
                // No necesitamos comprobar la sesión ni repositorios, solo asignar el IdUsuarioCrea desde el formulario
                if (pago.IdUsuarioCrea == null)
                {
                    ModelState.AddModelError("IdUsuarioCrea", "Debe seleccionar un usuario.");
                    return View(pago);
                }

                // Asignamos la fecha de creación
                pago.FechaCreacion = DateTime.Now;

                // Obtener el último número de pago para el contrato y calcular el nuevo número de pago
                var ultimoNumeroPago = _pagoRepository.ObtenerUltimoNumeroPago(pago.IdContrato);
                // Si no hay pagos previos, comenzamos con 1; de lo contrario, sumamos 1 al último número de pago
                pago.NumeroPagoContrato = (ultimoNumeroPago ?? 0) + 1;

                // Crear el pago
                _pagoRepository.Create(pago);

                return RedirectToAction(nameof(Index));
            }

            return View(pago);
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
                // Validación: asegurarse de que MotivoAnulacion no sea nulo o vacío
                if (string.IsNullOrEmpty(anularPagoDTO.MotivoAnulacion))
                {
                    return Json(
                        new
                        {
                            success = false,
                            message = "Debe proporcionar un motivo de anulación.",
                        }
                    );
                }

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

        // GET: Pago/ObtenerImporteContrato
        [HttpGet]
        public IActionResult ObtenerImporteContrato(int id)
        {
            try
            {
                var contrato = _contratoRepository.ObtenerPorId(id);
                if (contrato != null)
                {
                    return Json(new { success = true, monto = contrato.MontoMensual });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el importe del contrato: {ex.Message}");
                return Json(new { success = false });
            }
        }
    }
}
