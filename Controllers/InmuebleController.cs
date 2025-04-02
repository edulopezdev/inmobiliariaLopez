using Microsoft.AspNetCore.Mvc;
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
                _repositorio.Edit(inmueble);
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