using System.Security.Claims;
using BCrypt.Net;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaLopez.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IRepositorioUsuario _usuarioRepository;
        private readonly IWebHostEnvironment _environment;
        private const int _registrosPorPagina = 10;

        public UsuariosController(
            IRepositorioUsuario usuarioRepository,
            IWebHostEnvironment environment
        )
        {
            _usuarioRepository = usuarioRepository;
            _environment = environment;
        }

        // GET: Login
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Inmuebles"); // Redirige si ya está autenticado
            }
            return View();
        }

        // POST: Login
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string contrasena, string returnUrl)
        {
            try
            {
                // Verificar si el email o la contraseña están vacíos
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contrasena))
                {
                    ModelState.AddModelError("", "El email y la contraseña son requeridos.");
                    return View(); // Devuelve la vista con el error
                }

                var usuario = _usuarioRepository.ObtenerPorEmail(email);

                // Verifica que el usuario no sea nulo
                if (usuario == null)
                {
                    ModelState.AddModelError("", "Email o contraseña incorrectos");
                    return View(); // Devuelve la vista con el error
                }

                // Verifica que la contraseña hasheada no sea nula
                if (string.IsNullOrEmpty(usuario.ContrasenaHasheada))
                {
                    ModelState.AddModelError("", "Error al procesar la contraseña del usuario.");
                    return View(); // Devuelve la vista con el error
                }

                // Verifica la contraseña usando BCrypt
                if (!BCrypt.Net.BCrypt.Verify(contrasena, usuario.ContrasenaHasheada))
                {
                    ModelState.AddModelError("", "Email o contraseña incorrectos");
                    return View(); // Devuelve la vista con el error
                }

                // Verificar el rol del usuario
                Console.WriteLine($"Rol del usuario: '{usuario.Rol}'");
                // Crear los claims para la autenticación del usuario
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim("IdUsuario", usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Rol),
                    new Claim("Avatar", usuario.Avatar ?? "/images/default-avatar.png"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Iniciar sesión del usuario
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(1),
                    }
                );

                // Redirigir al usuario a la página de inicio de Inmuebles (puedes cambiarlo por la ruta que desees)
                return RedirectToAction("Index", "Inmuebles");
            }
            catch (Exception ex)
            {
                return View(); // Devuelve la vista con el error
            }
        }

        // GET: Lista de usuarios
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Index(int pagina = 1)
        {
            try
            {
                // Llamamos al repositorio para obtener la lista paginada de usuarios
                var usuarios = _usuarioRepository.Index(pagina);

                // Obtenemos la cantidad total de usuarios activos
                int totalRegistros = _usuarioRepository.ObtenerTotal(); // O _usuarioRepository.ObtenerTotalUsuariosActivos()

                // Calculamos el número total de páginas
                int totalPaginas = (int)Math.Ceiling((double)totalRegistros / _registrosPorPagina);

                // Pasamos la información de paginación a la vista
                ViewBag.PaginaActual = pagina;
                ViewBag.TotalPaginas = totalPaginas;

                // Pasamos la lista de usuarios de la página actual a la vista
                return View(usuarios);
            }
            catch (Exception ex)
            {
                // Si ocurre un error, mostramos el mensaje en TempData y redirigimos a la vista de error
                TempData["ErrorMessage"] = $"Error al cargar los usuarios: {ex.Message}";
                return RedirectToAction("Error");
            }
        }

        // GET: Crear usuario
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Crear usuario
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario, IFormFile? Avatar)
        {
            try
            {
                // Verificar si el modelo es válido
                if (!ModelState.IsValid)
                {
                    return View(usuario); // Retorna la vista con los errores
                }

                // VALIDACIÓN MANUAL DE CONTRASEÑA
                if (string.IsNullOrEmpty(usuario.ContrasenaHasheada))
                {
                    ModelState.AddModelError("ContrasenaHasheada", "La contraseña es obligatoria.");
                    return View(usuario);
                }

                // Hashear la contraseña
                usuario.ContrasenaHasheada = BCrypt.Net.BCrypt.HashPassword(
                    usuario.ContrasenaHasheada
                );

                // Llamar al método SubirAvatar, si se proporciona un archivo
                if (Avatar != null && Avatar.Length > 0)
                {
                    var avatarResult = SubirAvatar(usuario, Avatar);
                    if (!avatarResult.success)
                    {
                        // Si hubo algún error al subir el avatar, agregar el mensaje de error
                        ModelState.AddModelError("", avatarResult.message);
                        return View(usuario);
                    }

                    // Asignar la URL del avatar al usuario
                    usuario.Avatar = avatarResult.avatarUrl;
                }

                // Establecer fecha de creación y estado activo
                usuario.FechaCreacion = DateTime.Now;
                usuario.Activo = true;

                // Crear el usuario usando el repositorio
                _usuarioRepository.Create(usuario);

                // Redirigir a la vista "Index"
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "Hubo un error al crear el usuario. Por favor, inténtalo de nuevo."
                );
                return View(usuario);
            }
        }

        // Método auxiliar para manejar la subida de avatar
        private (bool success, string message, string avatarUrl) SubirAvatar(
            Usuario usuario,
            IFormFile Avatar
        )
        {
            try
            {
                // Validaciones de extensión de archivo
                var extensionesValidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(Avatar.FileName).ToLower();

                if (!extensionesValidas.Contains(extension))
                {
                    return (
                        false,
                        "Solo se permiten imágenes con extensiones .jpg, .jpeg, .png, .gif.",
                        string.Empty
                    );
                }

                // Generar nombre único para el archivo
                var numeroAleatorio = Guid.NewGuid().ToString("N").Substring(0, 8); // Usamos una parte del Guid para mayor unicidad
                var nombreArchivo =
                    $"avatarusuario_{usuario.IdUsuario}_{numeroAleatorio}{extension}";

                // Ruta base donde se guardarán los archivos
                string rutaBase = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "img",
                    "usuarios",
                    "avatar",
                    usuario.IdUsuario.ToString()
                );

                // Crear la carpeta si no existe
                Directory.CreateDirectory(rutaBase);

                // Ruta completa para guardar el archivo
                string rutaCompleta = Path.Combine(rutaBase, nombreArchivo);

                // Guardar el archivo en la carpeta correspondiente
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    Avatar.CopyTo(stream);
                }

                // URL relativa para el avatar
                string avatarUrl = $"/img/usuarios/avatar/{usuario.IdUsuario}/{nombreArchivo}";

                return (true, string.Empty, avatarUrl);
            }
            catch (Exception)
            {
                return (
                    false,
                    "Hubo un error al subir el avatar. Intenta nuevamente.",
                    string.Empty
                );
            }
        }

        // Acción GET para mostrar los detalles de un usuario
        public IActionResult Details(int id)
        {
            // Obtener el usuario por ID utilizando el repositorio
            var usuario = _usuarioRepository.Details(id);

            // Si no se encuentra el usuario, devolver un NotFound
            if (usuario == null)
            {
                return NotFound();
            }

            // Pasamos el usuario a la vista
            return View(usuario);
        }

        // GET: Editar usuario
        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = User.FindFirst("IdUsuario")?.Value;

            // Convertir id a string para comparación
            if (userId == null || (id.ToString() != userId && !User.IsInRole("Administrador")))
            {
                return Unauthorized(); // Si no es el mismo usuario o no es admin, se deniega el acceso
            }

            var usuario = _usuarioRepository.Details(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Editar usuario
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Usuario usuario, IFormFile? Avatar)
        {
            try
            {
                // Verificar si el modelo es válido
                if (!ModelState.IsValid)
                {
                    return View(usuario); // Retorna la vista con los errores
                }

                // Buscar el usuario existente
                var usuarioExistente = _usuarioRepository.Details(id);
                if (usuarioExistente == null)
                {
                    ModelState.AddModelError("", "Usuario no encontrado.");
                    return View(usuario);
                }

                // Verificar si el usuario es un Empleado y si está intentando editar su propio perfil
                if (User.IsInRole("Empleado") && usuario.IdUsuario != id)
                {
                    // Si el empleado está intentando editar otro usuario, redirigirlo
                    return RedirectToAction("AccessDenied", "Home");
                }

                // Si el usuario es un empleado, no permitir la modificación del rol
                if (User.IsInRole("Empleado"))
                {
                    usuario.Rol = usuarioExistente.Rol; // Mantener el rol actual
                }

                // Si se proporciona una nueva contraseña, la hasheamos
                if (!string.IsNullOrEmpty(usuario.NuevaContrasena))
                {
                    usuario.ContrasenaHasheada = BCrypt.Net.BCrypt.HashPassword(
                        usuario.NuevaContrasena
                    );
                }
                else
                {
                    // Si no se ha proporcionado una nueva contraseña, mantener la contraseña actual
                    usuario.ContrasenaHasheada = usuarioExistente.ContrasenaHasheada;
                }

                // Manejo del avatar
                if (Avatar != null)
                {
                    var resultadoSubida = SubirAvatar(usuario, Avatar);
                    if (!resultadoSubida.success)
                    {
                        ModelState.AddModelError("", resultadoSubida.message);
                        return View(usuario);
                    }
                    usuario.Avatar = resultadoSubida.avatarUrl;
                }
                else
                {
                    usuario.Avatar = usuarioExistente.Avatar;
                }

                // Otros datos que no vienen del formulario directamente
                usuario.FechaCreacion = usuarioExistente.FechaCreacion;
                usuario.Activo = usuarioExistente.Activo;

                // Guardar cambios
                _usuarioRepository.Edit(usuario);

                // Redirigir a Index
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar mensaje
                ModelState.AddModelError(
                    "",
                    "Hubo un error al editar el usuario. Por favor, inténtalo de nuevo."
                );
                return View(usuario);
            }
        }

        // GET: Eliminar usuario
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                // Obtener el usuario por ID
                var usuario = _usuarioRepository.Details(id);

                // Si el usuario no existe, devolver NotFound
                if (usuario == null)
                {
                    return NotFound();
                }

                // Pasar el usuario a la vista para confirmación
                return View(usuario);
            }
            catch (Exception ex)
            {
                // Si ocurre algún error, redirigir a una página de error
                TempData["ErrorMessage"] = $"Error al intentar cargar el usuario: {ex.Message}";
                return RedirectToAction("Error");
            }
        }

        // POST: Eliminar usuario
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Verificar si el usuario actual es un administrador y si está intentando eliminar su propia cuenta
                var currentUserId = User.FindFirst("IdUsuario")?.Value;

                if (currentUserId == id.ToString())
                {
                    // Si es el mismo usuario (administrador) que intenta eliminarse a sí mismo
                    ModelState.AddModelError(
                        "",
                        "No puedes eliminar tu propia cuenta de administrador."
                    );
                    return RedirectToAction("Index"); // Redirige a la lista de usuarios
                }

                // Llamar al método Delete del repositorio para eliminar al usuario
                _usuarioRepository.Delete(id);

                // Redirigir a la vista Index después de eliminar el usuario
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Si ocurre algún error, mostrar mensaje de error
                TempData["ErrorMessage"] = $"Error al eliminar el usuario: {ex.Message}";
                return RedirectToAction("Error");
            }
        }

        // Acción para cerrar sesión
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Cierra la sesión
            return RedirectToAction("Login", "Usuarios"); // Redirige a la vista de login en el controlador Usuarios
        }
    }
}
