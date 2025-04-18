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
        private readonly IRepositorioImagen _repoImagen;
        private readonly IWebHostEnvironment _environment; // Inyecta IWebHostEnvironment

        public InmueblesController(
            IRepositorioInmueble repositorio,
            IRepositorioPropietario repoPropietario,
            IRepositorioImagen repoImagen,
            IWebHostEnvironment environment // Recibe IWebHostEnvironment
        )
        {
            _repositorio = repositorio;
            _repoPropietario = repoPropietario;
            _repoImagen = repoImagen;
            _environment = environment; // Asigna a la variable de clase
        }

        // GET: Inmuebles
        public IActionResult Index()
        {
            var inmuebles = _repositorio.Index();
            var portadas = new Dictionary<int, string?>();

            foreach (var inmueble in inmuebles)
            {
                var portada = _repoImagen
                    .ObtenerPorInmueble(inmueble.IdInmueble)
                    .FirstOrDefault(img => img.TipoImagen == "InmueblePortada" && img.Activo);
                portadas[inmueble.IdInmueble] = portada?.Ruta;
            }

            ViewBag.Portadas = portadas; // ¡Esta línea es crucial!
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
                    "NombreCompleto"
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
                "NombreCompleto",
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
        public async Task<IActionResult> CreateOrEdit(int? id, Inmueble inmueble, IFormFile archivo)
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

                        // Subir la portada si se proporcionó un archivo
                        if (archivo != null && archivo.Length > 0)
                        {
                            await SubirPortada(inmueble.IdInmueble, archivo); // Llama a la acción SubirPortada
                        }
                    }
                    else
                    {
                        // Editar un inmueble existente
                        _repositorio.Edit(inmueble);
                        TempData["Mensaje"] = "Inmueble modificado correctamente";

                        // Subir la portada si se proporcionó un archivo
                        if (archivo != null && archivo.Length > 0)
                        {
                            await SubirPortada(inmueble.IdInmueble, archivo); // Llama a la acción SubirPortada
                        }
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

        // POST: Inmuebles/SubirPortada
        [HttpPost]
        public async Task<IActionResult> SubirPortada(int idInmueble, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                TempData["Error"] = "Debes seleccionar una imagen.";
                return RedirectToAction("Details", new { id = idInmueble });
            }

            // Buscar la portada activa existente para este inmueble
            var portadaExistente = _repoImagen
                .ObtenerPorInmueble(idInmueble)
                .FirstOrDefault(img => img.TipoImagen == "InmueblePortada" && img.Activo);

            if (portadaExistente != null)
            {
                // 1. Eliminar el archivo físico de la portada anterior
                string rutaArchivoAnterior = Path.Combine(
                    _environment.WebRootPath,
                    portadaExistente.Ruta.TrimStart('/')
                );
                if (System.IO.File.Exists(rutaArchivoAnterior))
                {
                    try
                    {
                        System.IO.File.Delete(rutaArchivoAnterior);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al eliminar la portada anterior: {ex.Message}");
                        TempData["Error"] = "Ocurrió un error al eliminar la portada anterior.";
                        return RedirectToAction("Details", new { id = idInmueble });
                    }
                }

                // 2. Eliminar el registro de la portada anterior de la base de datos
                _repoImagen.Delete(portadaExistente.IdImagen); // Asumiendo que agregaremos un método Delete al repositorio
            }

            // Generar el nuevo nombre y ruta para la portada
            var extension = Path.GetExtension(archivo.FileName);
            var numeroAleatorio = Guid.NewGuid().ToString().Substring(0, 8);
            string nombreUnico = $"portada_{idInmueble}_{numeroAleatorio}{extension}";
            string rutaCarpetaRelativa = "img/inmuebles";
            string rutaBase = _environment.WebRootPath;
            string rutaCarpetaCompleta = Path.Combine(
                rutaBase,
                rutaCarpetaRelativa,
                idInmueble.ToString()
            );
            Directory.CreateDirectory(rutaCarpetaCompleta);
            string rutaCompletaArchivo = Path.Combine(rutaCarpetaCompleta, nombreUnico);
            string rutaRelativaWeb = $"/img/inmuebles/{idInmueble}/{nombreUnico}";

            // Guardar el nuevo archivo físico
            using (var stream = new FileStream(rutaCompletaArchivo, FileMode.Create))
            {
                try
                {
                    await archivo.CopyToAsync(stream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al guardar la nueva portada: {ex.Message}");
                    TempData["Error"] = "Ocurrió un error al guardar la nueva portada.";
                    return RedirectToAction("Details", new { id = idInmueble });
                }
            }

            // 3. Crear y guardar el registro de la nueva portada en la base de datos
            var nuevaImagen = new Imagen
            {
                Ruta = rutaRelativaWeb,
                TipoImagen = "InmueblePortada",
                IdRelacionado = idInmueble,
                Activo = true,
            };

            _repoImagen.Create(nuevaImagen);

            TempData["Success"] = "Portada actualizada correctamente.";
            return RedirectToAction("Details", new { id = idInmueble });
        }
    }
}
