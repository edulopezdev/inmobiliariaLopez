using System;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmobiliariaLopez.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly IRepositorioInmueble _repositorio;
        private readonly IRepositorioPropietario _repoPropietario;

        public InmueblesController(
            IRepositorioInmueble repositorio,
            IRepositorioPropietario repoPropietario
        )
        {
            _repositorio = repositorio;
            _repoPropietario = repoPropietario;
        }

        // GET: Inmuebles
        public IActionResult Index()
        {
            var inmuebles = _repositorio.Index();
            return View(inmuebles);
        }

        // GET: Inmuebles/Details/5
        public IActionResult Details(int id)
        {
            var inmueble = _repositorio.Details(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            return View(inmueble);
        }

        // GET: Inmuebles/CreateOrEdit
        public IActionResult CreateOrEdit(int? id)
        {
            // Crear un nuevo inmueble si no se proporciona un ID
            if (id == null)
            {
                // Proporcionar la lista de propietarios para el campo desplegable
                ViewBag.Propietarios = new SelectList(
                    _repoPropietario.Index(),
                    "IdPropietario",
                    "Nombre"
                );

                // Proporcionar la lista de tipos de inmuebles para el campo desplegable
                ViewBag.TiposInmueble = new SelectList(
                    _repositorio.ObtenerTiposInmuebles(),
                    "IdTipoInmueble",
                    "Nombre"
                );

                return View(new Inmueble());
            }

            // Cargar el inmueble existente si se proporciona un ID
            var inmueble = _repositorio.Details(id.Value);
            if (inmueble == null)
            {
                return NotFound();
            }

            // Proporcionar la lista de propietarios para el campo desplegable
            ViewBag.Propietarios = new SelectList(
                _repoPropietario.Index(),
                "IdPropietario",
                "Nombre",
                inmueble.IdPropietario
            );

            // Proporcionar la lista de tipos de inmuebles para el campo desplegable
            ViewBag.TiposInmueble = new SelectList(
                _repositorio.ObtenerTiposInmuebles(),
                "IdTipoInmueble",
                "Nombre",
                inmueble.IdTipoInmueble
            );

            return View(inmueble);
        }

        // POST: Inmuebles/CreateOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int? id, Inmueble inmueble)
        {
            // Validar que el ID coincida con el modelo
            if (id != null && inmueble.IdInmueble != id)
            {
                return BadRequest("El ID del inmueble no coincide con el ID proporcionado.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null)
                    {
                        // Crear un nuevo inmueble
                        _repositorio.Create(inmueble);
                        TempData["Mensaje"] = "Inmueble creado correctamente";
                    }
                    else
                    {
                        // Editar un inmueble existente
                        _repositorio.Edit(inmueble);
                        TempData["Mensaje"] = "Inmueble modificado correctamente";
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(
                        "",
                        $"Ocurrió un error al guardar el inmueble: {ex.Message}"
                    );
                }
            }

            // Si hay errores, volvemos a cargar la vista con los datos actuales
            ViewBag.Propietarios = new SelectList(
                _repoPropietario.Index(),
                "IdPropietario",
                "Nombre",
                inmueble.IdPropietario
            );

            return View(inmueble);
        }

        // GET: Inmuebles/Delete/5
        public IActionResult Delete(int id)
        {
            var inmueble = _repositorio.Details(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            return View(inmueble);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repositorio.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Inmuebles/PorPropietario/1
        public IActionResult PorPropietario(int idPropietario)
        {
            var inmuebles = _repositorio.ObtenerPorPropietario(idPropietario);
            if (inmuebles == null || inmuebles.Count == 0)
            {
                TempData["Mensaje"] = "No hay inmuebles asociados al propietario seleccionado.";
            }
            return View("Index", inmuebles);
        }
    }
}
