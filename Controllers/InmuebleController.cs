using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Repositories;

namespace InmobiliariaLopez.Controllers
{
  public class InmueblesController : Controller
  {
    private readonly IRepositorioInmueble _repositorio;

    // Constructor para inyectar el repositorio
    public InmueblesController(IRepositorioInmueble repositorio)
    {
      _repositorio = repositorio;
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

    // GET: Inmuebles/CreateOrEdit (para Crear o Editar)
    public IActionResult CreateOrEdit(int? id)
    {
      // Obtener tipos de inmuebles para el dropdown
      var tiposInmuebles = _repositorio.ObtenerTiposInmuebles();
      ViewBag.TipoInmuebles = new SelectList(tiposInmuebles, "IdTipoInmueble", "Nombre");

      if (id == null)
      {
        // Si no se proporciona un ID, es una creación
        return View(new Inmueble());
      }

      // Si se proporciona un ID, es una edición
      var inmueble = _repositorio.Details(id.Value);
      if (inmueble == null)
      {
        return NotFound();
      }

      return View(inmueble);
    }

    // POST: Inmuebles/CreateOrEdit (para Crear o Editar)
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
            }
            else
            {
                // Actualizar un inmueble existente
                _repositorio.Edit(inmueble);
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Ocurrió un error al guardar el inmueble: {ex.Message}");
        }
    }

    // Si hay errores, volvemos a cargar la vista con los datos actuales
    var tiposInmuebles = _repositorio.ObtenerTiposInmuebles();
    ViewBag.TipoInmuebles = new SelectList(tiposInmuebles, "IdTipoInmueble", "Nombre", inmueble.IdTipoInmueble);

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

    // GET: Inmuebles/VerificarDisponibilidad/1
    public IActionResult VerificarDisponibilidad(int idInmueble, DateTime fechaInicio, DateTime fechaFin)
    {
      var disponible = _repositorio.EstaDisponibleEnFechas(idInmueble, fechaInicio, fechaFin);
      if (disponible)
      {
        TempData["Mensaje"] = "El inmueble está disponible en el rango de fechas seleccionado.";
      }
      else
      {
        TempData["Mensaje"] = "El inmueble NO está disponible en el rango de fechas seleccionado.";
      }
      return RedirectToAction(nameof(Index));
    }
  }
}