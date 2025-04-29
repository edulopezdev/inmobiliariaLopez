using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace InmobiliariaLopez.Controllers
{
    public class ContratosController : Controller
    {
        private readonly IRepositorioContrato _repositorioContrato;
        private readonly IRepositorioInmueble _repositorioInmueble;
        private readonly IRepositorioInquilino _repositorioInquilino;

        public ContratosController(
            IRepositorioContrato repositorioContrato,
            IRepositorioInmueble repositorioInmueble,
            IRepositorioInquilino repositorioInquilino
        )
        {
            _repositorioContrato = repositorioContrato;
            _repositorioInmueble = repositorioInmueble;
            _repositorioInquilino = repositorioInquilino;
        }

        // GET: Pago
        public IActionResult Index(int pagina = 1)
        {
            var contratos = _repositorioContrato.Index(pagina);
            int cantidadTotal = _repositorioContrato.ObtenerTotal();
            int registrosPorPagina = 10;
            int totalPaginas = (int)Math.Ceiling((double)cantidadTotal / registrosPorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

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

        // GET: Contratos/Create
        public IActionResult Create()
        {
            CargarViewBag(); // Llamamos al método para cargar los ViewBag
            var contrato = new Contrato
            {
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddYears(1),
                EstadoContrato = "Vigente",
            };
            return View(contrato);
        }

        // POST: Contratos/Create
        [HttpPost]
        public IActionResult Create(Contrato contrato)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    );
                return Json(new { success = false, errors });
            }

            var fechasOcupadas = _repositorioContrato.ControlFechas(
                contrato.IdInmueble,
                contrato.FechaInicio,
                contrato.FechaFin
            );

            if (fechasOcupadas.Any())
            {
                string mensaje = "El inmueble ya tiene contratos en las siguientes fechas:<br/>";
                mensaje += string.Join(
                    "<br/>",
                    fechasOcupadas.Select(f => $"• {f.Item1:dd/MM/yyyy} al {f.Item2:dd/MM/yyyy}")
                );

                return Json(new { success = false, message = mensaje });
            }

            _repositorioContrato.Create(contrato);
            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }

        // GET: Contratos/Edit/5
        public IActionResult Edit(int id)
        {
            var contrato = _repositorioContrato.Details(id);
            if (contrato == null)
            {
                return NotFound();
            }

            CargarViewBag(contrato.IdInmueble, contrato.IdInquilino); // Pasamos los IDs para seleccionar los valores correctos
            ViewBag.EstadosContrato = new SelectList(
                new[] { "Vigente", "Finalizado", "Anulado" },
                contrato.EstadoContrato
            );
            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        public IActionResult Edit(Contrato contrato)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos." });
            }

            var contratoExistente = _repositorioContrato.ObtenerPorId(contrato.IdContrato);
            if (contratoExistente == null)
            {
                return NotFound();
            }

            var fechasOcupadas = _repositorioContrato.ControlFechas(
                contrato.IdInmueble,
                contrato.FechaInicio,
                contrato.FechaFin,
                contrato.IdContrato // se excluye a sí mismo
            );

            if (fechasOcupadas.Any())
            {
                string mensaje = "Ya existe otro contrato activo en esas fechas:<br/>";
                mensaje += string.Join(
                    "<br/>",
                    fechasOcupadas.Select(f => $"• {f.Item1:dd/MM/yyyy} al {f.Item2:dd/MM/yyyy}")
                );

                return Json(new { success = false, message = mensaje });
            }

            // Actualizar y guardar los cambios si todo va bien...
            contratoExistente.FechaInicio = contrato.FechaInicio;
            contratoExistente.FechaFin = contrato.FechaFin;
            contratoExistente.MontoMensual = contrato.MontoMensual;
            contratoExistente.IdInquilino = contrato.IdInquilino;
            contratoExistente.IdInmueble = contrato.IdInmueble;

            _repositorioContrato.Edit(contratoExistente);

            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }

        // GET: Contratos/Delete/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            try
            {
                Console.WriteLine(
                    $"Recibiendo solicitud de eliminación para el contrato con ID: {id}"
                );

                var contrato = _repositorioContrato.Details(id);

                if (contrato == null)
                {
                    Console.WriteLine($"No se encontró el contrato con ID: {id}.");
                    return NotFound($"No se encontró el contrato con ID {id}."); // Mostrar error si no se encuentra
                }

                return View(contrato);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el contrato: {ex.Message}");
                TempData["ErrorMessage"] = $"Error al cargar el Contrato: {ex.Message}";
                return RedirectToAction("Error"); // En caso de error, redirigir a página de error
            }
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var contrato = _repositorioContrato.ObtenerPorId(id);
            if (contrato == null)
            {
                // Devolver un JSON de error
                return Json(new { success = false, message = "Contrato no encontrado." });
            }

            int rowsAffected = _repositorioContrato.Delete(id);
            if (rowsAffected > 0)
            {
                // Devolver un JSON de éxito
                return Json(
                    new
                    {
                        success = true,
                        message = "El contrato ha sido marcado como inactivo.",
                        data = contrato,
                    }
                );
            }
            else
            {
                // Devolver un JSON de error
                return Json(new { success = false, message = "No se pudo eliminar el contrato." });
            }
        }

        // GET: Contratos/Anular
        public IActionResult Anular(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contrato = _repositorioContrato.Details(id.Value);
            if (contrato == null)
            {
                return NotFound();
            }

            return View(contrato); // esto carga Anular.cshtml
        }

        // POST: Contratos/AnularContrato
        [HttpPost]
        public IActionResult AnularContrato([FromBody] Contrato contrato)
        {
            try
            {
                // Buscar el contrato en la base de datos
                var contratoExistente = _repositorioContrato.ObtenerPorId(contrato.IdContrato);
                if (contratoExistente == null)
                {
                    return Json(new { success = false, message = "Contrato no encontrado." });
                }

                // Verifica si el contrato ya está en estado 'PendienteAnulacion'
                if (contratoExistente.EstadoContrato == "PendienteAnulacion")
                {
                    return Json(
                        new
                        {
                            success = false,
                            message = "Este contrato ya está en proceso de anulación.",
                        }
                    );
                }

                // Actualizar el contrato: cambiar estado a 'PendienteAnulacion'
                contratoExistente.EstadoContrato = "PendienteAnulacion";
                contratoExistente.FechaRescision = contrato.FechaRescision; // Usamos la fecha de rescisión proporcionada
                contratoExistente.IdUsuarioAnula = contrato.IdUsuarioAnula;
                contratoExistente.FechaUsuarioAnula = DateTime.Now; // Fecha y hora de la anulación
                contratoExistente.Observaciones = contrato.Observaciones;
                contratoExistente.Activo = true; // No desactivamos el contrato aún, solo lo marcamos como pendiente de anulación

                // Llamar al método de repositorio para realizar la actualización en la base de datos
                var filasAfectadas = _repositorioContrato.AnularContrato(contratoExistente);

                // Verificar si la actualización fue exitosa
                if (filasAfectadas > 0)
                {
                    return Json(
                        new { success = true, redirectUrl = Url.Action("Index", "Contratos") }
                    );
                }
                else
                {
                    return Json(
                        new { success = false, message = "No se pudo actualizar el contrato." }
                    );
                }
            }
            catch (Exception ex)
            {
                // Capturar cualquier error y devolver un mensaje de error
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // Método privado para cargar el ViewBag con los datos necesarios
        private void CargarViewBag(int? inmuebleId = null, int? inquilinoId = null)
        {
            ViewBag.Inmuebles = new SelectList(
                _repositorioInmueble.Index(),
                "IdInmueble",
                "Direccion",
                inmuebleId // Pasamos el ID seleccionado para que se mantenga en el dropdown
            );
            ViewBag.Inquilinos = new SelectList(
                _repositorioInquilino
                    .Index()
                    .Select(i => new
                    {
                        i.IdInquilino,
                        NombreCompleto = $"{i.Nombre} {i.Apellido}",
                    }),
                "IdInquilino",
                "NombreCompleto",
                inquilinoId // Pasamos el ID seleccionado para que se mantenga en el dropdown
            );
        }

        // POST: Contratos/VerificarDisponibilidad
        [HttpPost]
        public IActionResult VerificarDisponibilidad([FromBody] Contrato contrato)
        {
            if (contrato == null || contrato.IdInmueble <= 0)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = "Datos incompletos para verificar disponibilidad.",
                    }
                );
            }

            // Llamar al método ControlFechas
            var fechasOcupadas = _repositorioContrato.ControlFechas(
                contrato.IdInmueble,
                contrato.FechaInicio,
                contrato.FechaFin,
                contrato.IdContrato
            );

            // Verificar si hay fechas ocupadas
            if (fechasOcupadas.Any())
            {
                string mensajeError =
                    "El inmueble no está disponible en las fechas seleccionadas. Ya está ocupado entre las siguientes fechas:\n";

                // Formatear las fechas ocupadas
                mensajeError += string.Join(
                    "\n",
                    fechasOcupadas.Select(f => $"{f.Item1:dd/MM/yyyy} al {f.Item2:dd/MM/yyyy}")
                );

                return Json(new { success = false, message = mensajeError });
            }

            return Json(new { success = true });
        }
    }
}
