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

        public IActionResult Details(int id)
        {
            var inmueble = _repositorio.Details(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            // Obtener las imagenes asociadas al inmueble
            inmueble.Imagenes = _repoImagen.ObtenerPorInmueble(id);

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

        // GET: Inmuebles/GaleriaImagenes
        [HttpGet]
        public IActionResult GaleriaImagenes(int idInmueble)
        {
            var inmueble = _repositorio.Details(idInmueble);
            if (inmueble == null)
                return NotFound();

            inmueble.Imagenes = _repoImagen.ObtenerPorInmueble(idInmueble);
            return PartialView("_GaleriaImagenes", inmueble);
        }

        // POST: Inmuebles/GuardarImagen
        [HttpPost]
        public async Task<IActionResult> GuardarImagen(
            int idInmueble,
            bool esReemplazoPortada,
            List<IFormFile> archivos
        )
        {
            if (archivos == null || archivos.Count == 0)
            {
                TempData["Error"] = "Debes seleccionar al menos una imagen.";
                return RedirectToAction("Details", new { id = idInmueble });
            }

            try
            {
                foreach (var archivo in archivos)
                {
                    // Validar cada archivo
                    if (
                        !archivo.ContentType.StartsWith("image/")
                        || archivo.Length > 5 * 1024 * 1024
                    )
                    {
                        TempData["Error"] = "Todas las imágenes deben ser válidas y menores a 5MB.";
                        continue; // Salta este archivo inválido y sigue con los demás
                    }

                    if (esReemplazoPortada)
                    {
                        // Solo subimos una portada, así que llamamos y salimos
                        await SubirPortada(idInmueble, archivo);
                        break;
                    }
                    else
                    {
                        var extension = Path.GetExtension(archivo.FileName);
                        var numeroAleatorio = Guid.NewGuid().ToString("N").Substring(0, 8);
                        string nombreUnico =
                            $"inmuebleinterior_{idInmueble}_{numeroAleatorio}{extension}";
                        string rutaCarpetaRelativa = "img/inmuebles/galeria";
                        string rutaBase = _environment.WebRootPath;
                        string rutaCarpetaCompleta = Path.Combine(rutaBase, rutaCarpetaRelativa);

                        // Crear carpeta si no existe
                        Directory.CreateDirectory(rutaCarpetaCompleta);

                        string rutaCompletaArchivo = Path.Combine(rutaCarpetaCompleta, nombreUnico);
                        string rutaRelativaWeb = $"/img/inmuebles/galeria/{nombreUnico}";

                        // Guardar archivo físico
                        using (var stream = new FileStream(rutaCompletaArchivo, FileMode.Create))
                        {
                            await archivo.CopyToAsync(stream);
                        }

                        // Registrar en la base de datos
                        var nuevaImagen = new Imagen
                        {
                            Ruta = rutaRelativaWeb,
                            TipoImagen = "InmuebleInterior",
                            IdRelacionado = idInmueble,
                            Activo = true,
                        };
                        _repoImagen.Create(nuevaImagen);
                    }
                }

                TempData["Success"] = "Imágenes subidas correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error al guardar las imágenes: {ex.Message}";
                Console.WriteLine($"Error en GuardarImagen: {ex.Message}");
            }

            return RedirectToAction("Details", new { id = idInmueble });
        }

        // POST: Inmuebles/EliminarImagen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarImagen(int idImagen, int idInmueble)
        {
            try
            {
                var imagen = _repoImagen.Details(idImagen);
                if (imagen == null)
                {
                    return Json(
                        new
                        {
                            success = false,
                            message = "La imagen no existe o ya ha sido eliminada.",
                        }
                    );
                }

                string rutaArchivo = Path.Combine(
                    _environment.WebRootPath,
                    imagen.Ruta.TrimStart('/')
                );
                if (System.IO.File.Exists(rutaArchivo))
                {
                    System.IO.File.Delete(rutaArchivo);
                }

                _repoImagen.Delete(imagen.IdImagen);

                return Json(new { success = true, message = "Imagen eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(
                    new { success = false, message = "Ocurrió un error al eliminar la imagen." }
                );
            }
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

            try
            {
                // Validar que sea una imagen válida y no exceda el tamaño máximo (5MB)
                if (!archivo.ContentType.StartsWith("image/") || archivo.Length > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "Solo se permiten imágenes menores a 5MB.";
                    return RedirectToAction("Details", new { id = idInmueble });
                }

                // Buscar la portada activa existente para este inmueble
                var portadaExistente = _repoImagen
                    .ObtenerPorInmueble(idInmueble)
                    .FirstOrDefault(img => img.TipoImagen == "InmueblePortada" && img.Activo);

                if (portadaExistente != null)
                {
                    // Eliminar el archivo físico de la portada anterior
                    string rutaArchivoAnterior = Path.Combine(
                        _environment.WebRootPath,
                        portadaExistente.Ruta.TrimStart('/')
                    );
                    if (System.IO.File.Exists(rutaArchivoAnterior))
                    {
                        System.IO.File.Delete(rutaArchivoAnterior);
                    }

                    // Eliminar el registro de la portada anterior de la base de datos
                    _repoImagen.Delete(portadaExistente.IdImagen);
                }

                // Generar el nuevo nombre y ruta para la portada
                var extension = Path.GetExtension(archivo.FileName);
                var numeroAleatorio = Guid.NewGuid().ToString("N").Substring(0, 8);
                string nombreUnico = $"inmuebleportada_{idInmueble}_{numeroAleatorio}{extension}";
                string rutaCarpetaRelativa = "img/inmuebles/portada";
                string rutaBase = _environment.WebRootPath;
                string rutaCarpetaCompleta = Path.Combine(rutaBase, rutaCarpetaRelativa);

                Directory.CreateDirectory(rutaCarpetaCompleta); // Crear carpeta si no existe
                string rutaCompletaArchivo = Path.Combine(rutaCarpetaCompleta, nombreUnico);
                string rutaRelativaWeb = $"/img/inmuebles/portada/{nombreUnico}";

                // Guardar el nuevo archivo físico
                using (var stream = new FileStream(rutaCompletaArchivo, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                // Crear y guardar el registro de la nueva portada en la base de datos
                var nuevaImagen = new Imagen
                {
                    Ruta = rutaRelativaWeb,
                    TipoImagen = "InmueblePortada",
                    IdRelacionado = idInmueble,
                    Activo = true,
                };
                _repoImagen.Create(nuevaImagen);

                TempData["Success"] = "Portada actualizada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error al actualizar la portada: {ex.Message}";
                Console.WriteLine($"Error en SubirPortada: {ex.Message}");
            }

            return RedirectToAction("Details", new { id = idInmueble });
        }
    }
}
