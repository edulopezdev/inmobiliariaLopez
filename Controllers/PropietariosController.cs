using InmobiliariaLopez.Models;
using InmobiliariaLopez.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaLopez.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly IRepositorioPropietario _propietarioRepository;

        // Constructor para inyectar el repositorio
        public PropietariosController(IRepositorioPropietario propietarioRepository)
        {
            _propietarioRepository = propietarioRepository;
        }

        // GET: Propietarios
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var propietarios = _propietarioRepository.Index();
                return View(propietarios);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los propietarios: {ex.Message}";
                return RedirectToAction("Error");
            }
        }

        // GET: Propietarios/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Retorna la vista Create.cshtml
        }

        // POST: Propietarios/Create
        [HttpPost]
        public IActionResult Create(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                _propietarioRepository.Create(propietario); // Guarda el propietario en la base de datos
                return RedirectToAction(nameof(Index)); // Redirige a la lista de propietarios
            }
            return View(propietario); // Si hay errores de validación, vuelve a mostrar la vista con los datos ingresados
        }

        // GET: Buscar Propietario
        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var propietario = _propietarioRepository.Details(id); // Obtener el propietario por ID
                if (propietario == null)
                {
                    return NotFound($"No se encontró ningún propietario con el ID {id}.");
                }
                return View(propietario); // Asegúrate de tener una vista 'Details.cshtml'
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el propietario: {ex.Message}";
                return RedirectToAction("Error");
            }
        }

        // GET: Propietarios/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var propietario = _propietarioRepository.Details(id); // Obtener el propietario por ID
                if (propietario == null)
                {
                    return NotFound($"No se encontró ningún propietario con el ID {id}.");
                }
                return View(propietario); // Pasar el propietario a la vista
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el propietario: {ex.Message}";
                return RedirectToAction("Error");
            }
        }

        // POST: Propietarios/Edit/{id}
        [HttpPost]
        public IActionResult Edit(int id, Propietario propietario)
        {
            if (id != propietario.IdPropietario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _propietarioRepository.Edit(propietario); // Actualizar el propietario en la base de datos
                    return RedirectToAction(nameof(Index)); // Redirigir a la lista de propietarios
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al actualizar el propietario: {ex.Message}";
                    return View(propietario);
                }
            }
            return View(propietario); // Si hay errores de validación, volver a mostrar la vista
        }

        // GET: Propietarios/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var propietario = _propietarioRepository.Details(id); // Obtener los detalles del propietario por ID
                if (propietario == null)
                {
                    return NotFound($"No se encontró el propietario con ID {id}."); // Mostrar error si no se encuentra
                }
                return View(propietario); // Pasar el propietario a la vista de confirmación
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el propietario: {ex.Message}";
                return RedirectToAction("Error"); // En caso de error, redirigir a página de error
            }
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Propietario propietario)
        {
            // Imprime el id recibido y el id del modelo Propietario
            Console.WriteLine($"ID recibido en la URL: {id}");
            Console.WriteLine($"ID recibido en el formulario: {propietario.IdPropietario}");

            if (id != propietario.IdPropietario)
            {
                return BadRequest("ID del propietario no coincide." + propietario.IdPropietario);
            }

            // Realizar la eliminación
            int rowsAffected = _propietarioRepository.Delete(id);

            if (rowsAffected > 0)
            {
                TempData["SuccessMessage"] =
                    $"El propietario con ID {id} ha sido eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] =
                    $"No se pudo encontrar el propietario con ID {id} para eliminar.";
            }

            return RedirectToAction(nameof(Index)); // Redirigir a la lista de propietarios
        }

        // GET: Propietarios/DNI/{dni}
        [HttpGet]
        public IActionResult ObtenerPorDNI(string dni)
        {
            try
            {
                var propietario = _propietarioRepository.ObtenerPorDNI(dni);
                if (propietario == null)
                {
                    return NotFound($"No se encontró ningún propietario con el DNI {dni}.");
                }
                return Ok(propietario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
