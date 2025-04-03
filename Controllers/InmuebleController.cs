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

    // GET: Inmuebles/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Inmuebles/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Inmueble inmueble)
    {
      if (ModelState.IsValid)
      {
        _repositorio.Create(inmueble);
        return RedirectToAction(nameof(Index));
      }
      return View(inmueble);
    }

    // GET: Inmuebles/Edit/5
    public IActionResult Edit(int id)
    {
      var inmueble = _repositorio.Details(id);
      if (inmueble == null)
      {
        return NotFound();
      }

      // Obtener los tipos de inmuebles y pasarlos a la vista
      var tiposInmuebles = _repositorio.ObtenerTiposInmuebles();
      if (tiposInmuebles == null || !tiposInmuebles.Any())
      {
        ModelState.AddModelError("", "No hay tipos de inmuebles disponibles.");
        return View(inmueble);
      }

      // Asignar los tipos de inmuebles a ViewBag
      ViewBag.TipoInmuebles = new SelectList(tiposInmuebles, "IdTipoInmueble", "Nombre", inmueble.IdTipoInmueble);

      return View(inmueble);
    }

    // POST: Inmuebles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Inmueble inmueble)
    {
      if (id != inmueble.IdInmueble)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        _repositorio.Edit(inmueble); // Asegúrate de que el método `Edit` esté bien implementado en el repositorio
        return RedirectToAction(nameof(Index));
      }
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
  }
}