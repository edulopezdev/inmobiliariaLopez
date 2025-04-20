using InmobiliariaLopez.Models;
using InmobiliariaLopez.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaLopez.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly IRepositorioInquilino _InquilinoRepository;

        // Constructor para inyectar el repositorio
        public InquilinosController(IRepositorioInquilino InquilinoRepository)
        {
            _InquilinoRepository = InquilinoRepository;
        }

        // GET: Inquilinos
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var Inquilinos = _InquilinoRepository.Index();
                return View(Inquilinos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los Inquilinos: {ex.Message}";
                return RedirectToAction("Error");
            }
        }

        // GET: Inquilinos/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Retorna la vista Create.cshtml
        }

        // POST: Inquilinos/Create
        [HttpPost]
        public IActionResult Create(Inquilino Inquilino)
        {
            if (ModelState.IsValid)
            {
                _InquilinoRepository.Create(Inquilino); // Guarda el Inquilino en la base de datos
                return RedirectToAction(nameof(Index)); // Redirige a la lista de Inquilinos
            }
            return View(Inquilino); // Si hay errores de validación, vuelve a mostrar la vista con los datos ingresados
        }

        // GET: Buscar Inquilino
        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var Inquilino = _InquilinoRepository.Details(id); // Obtener el Inquilino por ID
                if (Inquilino == null)
                {
                    return NotFound($"No se encontró ningún Inquilino con el ID {id}.");
                }
                return View(Inquilino); // Asegúrate de tener una vista 'Details.cshtml'
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el Inquilino: {ex.Message}";
                return RedirectToAction("Error");
            }
        }

        // GET: Inquilinos/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var Inquilino = _InquilinoRepository.Details(id); // Obtener el Inquilino por ID
                if (Inquilino == null)
                {
                    return NotFound($"No se encontró ningún Inquilino con el ID {id}.");
                }
                return View(Inquilino); // Pasar el Inquilino a la vista
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el Inquilino: {ex.Message}";
                return RedirectToAction("Error");
            }
        }

        // POST: Inquilinos/Edit/{id}
        [HttpPost]
        public IActionResult Edit(int id, Inquilino Inquilino)
        {
            if (id != Inquilino.IdInquilino)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _InquilinoRepository.Edit(Inquilino); // Actualizar el Inquilino en la base de datos
                    return RedirectToAction(nameof(Index)); // Redirigir a la lista de Inquilinos
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al actualizar el Inquilino: {ex.Message}";
                    return View(Inquilino);
                }
            }
            return View(Inquilino); // Si hay errores de validación, volver a mostrar la vista
        }

        // GET: Inquilinos/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var Inquilino = _InquilinoRepository.Details(id); // Obtener los detalles del Inquilino por ID
                if (Inquilino == null)
                {
                    return NotFound($"No se encontró el Inquilino con ID {id}."); // Mostrar error si no se encuentra
                }
                return View(Inquilino); // Pasar el Inquilino a la vista de confirmación
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el Inquilino: {ex.Message}";
                return RedirectToAction("Error"); // En caso de error, redirigir a página de error
            }
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Inquilino Inquilino)
        {
            // Imprime el id recibido y el id del modelo Inquilino
            Console.WriteLine($"ID recibido en la URL: {id}");
            Console.WriteLine($"ID recibido en el formulario: {Inquilino.IdInquilino}");

            if (id != Inquilino.IdInquilino)
            {
                return BadRequest("ID del Inquilino no coincide." + Inquilino.IdInquilino);
            }

            // Realizar la eliminación
            int rowsAffected = _InquilinoRepository.Delete(id);

            if (rowsAffected > 0)
            {
                TempData["SuccessMessage"] =
                    $"El Inquilino con ID {id} ha sido eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] =
                    $"No se pudo encontrar el Inquilino con ID {id} para eliminar.";
            }

            return RedirectToAction(nameof(Index)); // Redirigir a la lista de Inquilinos
        }

        // GET: Inquilinos/DNI/{dni}
        [HttpGet]
        public IActionResult ObtenerPorDNI(string dni)
        {
            try
            {
                var Inquilino = _InquilinoRepository.ObtenerPorDNI(dni);
                if (Inquilino == null)
                {
                    return NotFound($"No se encontró ningún Inquilino con el DNI {dni}.");
                }
                return Ok(Inquilino);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
