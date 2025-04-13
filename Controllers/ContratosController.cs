using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Repositories;
using InmobiliariaLopez.Services;
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
        private readonly IContratoService _contratoService;
        private readonly ILogger<ContratosController> _logger;

        public ContratosController(
            IRepositorioContrato repositorioContrato,
            IRepositorioInmueble repositorioInmueble,
            IRepositorioInquilino repositorioInquilino,
            IContratoService contratoService,
            ILogger<ContratosController> logger
        )
        {
            _repositorioContrato = repositorioContrato;
            _repositorioInmueble = repositorioInmueble;
            _repositorioInquilino = repositorioInquilino;
            _contratoService = contratoService;
            _logger = logger;
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                var resultado = _contratoService.VerificarDisponibilidad(
                    contrato.IdInmueble,
                    contrato.FechaInicio,
                    contrato.FechaFin
                );

                if (!resultado.Disponible)
                {
                    string mensajeError =
                        "El inmueble no está disponible en las fechas seleccionadas. Ya está ocupado entre las siguientes fechas: ";
                    mensajeError += string.Join(
                        "\\n", // Usamos \n para saltos de línea en SweetAlert
                        resultado.FechasOcupadas.Select(f =>
                            $"{f.Item1:dd/MM/yyyy} al {f.Item2:dd/MM/yyyy}"
                        )
                    );
                    return Json(new { success = false, message = mensajeError }); // Devolvemos JSON para AJAX
                }

                _repositorioContrato.Create(contrato);
                return Json(new { success = true, redirectUrl = Url.Action("Index") }); // Devolvemos JSON para AJAX
            }

            CargarViewBag(); //Recargamos el viewbag para que la vista tenga los datos
            return View(contrato); // Volvemos a la vista con el modelo y los errores
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
            if (ModelState.IsValid)
            {
                var contratoExistente = _repositorioContrato.ObtenerPorId(contrato.IdContrato);
                if (contratoExistente == null)
                {
                    return NotFound();
                }

                bool huboCambios = false;

                if (contrato.FechaInicio != contratoExistente.FechaInicio)
                {
                    contratoExistente.FechaInicio = contrato.FechaInicio;
                    huboCambios = true;
                }

                if (contrato.FechaFin != contratoExistente.FechaFin)
                {
                    contratoExistente.FechaFin = contrato.FechaFin;
                    huboCambios = true;
                }

                if (contrato.MontoMensual != contratoExistente.MontoMensual)
                {
                    contratoExistente.MontoMensual = contrato.MontoMensual;
                    huboCambios = true;
                }

                if (contrato.Multa != contratoExistente.Multa)
                {
                    contratoExistente.Multa = contrato.Multa;
                    huboCambios = true;
                }

                if (contrato.EstadoContrato != contratoExistente.EstadoContrato)
                {
                    contratoExistente.EstadoContrato = contrato.EstadoContrato;
                    huboCambios = true;
                }

                if (contrato.Observaciones != contratoExistente.Observaciones)
                {
                    contratoExistente.Observaciones = contrato.Observaciones;
                    huboCambios = true;
                }

                // Lógica para la anulación (modificando directamente el contratoExistente)
                if (
                    Request.Form.ContainsKey("AnularContrato")
                    && !string.IsNullOrEmpty(Request.Form["MotivoAnulacion"])
                )
                {
                    contratoExistente.Activo = false;
                    contratoExistente.FechaRescision = DateTime.Now; // O la fecha que corresponda
                    contratoExistente.Observaciones = $"ANULADO: {Request.Form["MotivoAnulacion"]}";
                    huboCambios = true;
                }
                else if (
                    contratoExistente.Activo == false
                    && !Request.Form.ContainsKey("AnularContrato")
                )
                {
                    contratoExistente.Activo = true;
                    contratoExistente.FechaRescision = null;
                    if (contratoExistente.Observaciones?.StartsWith("ANULADO:") == true)
                    {
                        contratoExistente.Observaciones = contratoExistente
                            .Observaciones.Substring("ANULADO: ".Length)
                            .Trim();
                        huboCambios = true;
                    }
                }

                if (huboCambios)
                {
                    _repositorioContrato.Edit(contratoExistente);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index"); // No hubo cambios, redirigir
                }
            }

            CargarViewBag(contrato.IdInmueble, contrato.IdInquilino);
            ViewBag.EstadosContrato = new SelectList(
                new[] { "Vigente", "Finalizado", "Anulado" },
                contrato.EstadoContrato
            );
            return View(contrato);
        }

        // GET: Contratos/Delete/5
        public IActionResult Delete(int id)
        {
            var contrato = _repositorioContrato.Details(id);
            if (contrato == null)
            {
                return NotFound();
            }
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repositorioContrato.Delete(id);
            TempData["Mensaje"] = "Contrato eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

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
    }
}
