using System.Security.Claims;
using BCrypt.Net;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaLopez.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IRepositorioUsuario _usuarioRepository;

        public UsuariosController(IRepositorioUsuario usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // GET: Usuarios
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                // Llamamos al repositorio para obtener la lista de usuarios
                var usuarios = _usuarioRepository.Index();

                // Pasamos la lista de usuarios a la vista
                return View(usuarios);
            }
            catch (Exception ex)
            {
                // Si ocurre un error, mostramos el mensaje en TempData y redirigimos a la vista de error
                TempData["ErrorMessage"] = $"Error al cargar los usuarios: {ex.Message}";
                return RedirectToAction("Error");
            }
        }

        // GET: Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // GET: Usuarios
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
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

                // Hashear la contraseña
                usuario.ContrasenaHasheada = BCrypt.Net.BCrypt.HashPassword(
                    usuario.ContrasenaHasheada
                );

                // Manejo del archivo Avatar
                if (Avatar != null && Avatar.Length > 0)
                {
                    // Verificar si la extensión del archivo es válida
                    var extensionesValidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(Avatar.FileName).ToLower();
                    if (!extensionesValidas.Contains(extension))
                    {
                        ModelState.AddModelError(
                            "",
                            "Solo se permiten imágenes con extensiones .jpg, .jpeg, .png, .gif."
                        );
                        return View(usuario);
                    }

                    var nombreArchivo = Guid.NewGuid().ToString() + extension; // Nombre único para el archivo
                    var ruta = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads",
                        nombreArchivo
                    );

                    // Guardar el archivo en el servidor
                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        Avatar.CopyTo(stream);
                    }

                    // Asignar la ruta del archivo al usuario
                    usuario.Avatar = "/uploads/" + nombreArchivo;
                }

                // Asignar fecha de creación y activar el usuario
                usuario.FechaCreacion = DateTime.Now;
                usuario.Activo = true;

                // Guardar el usuario en el repositorio
                _usuarioRepository.Create(usuario);

                // Redirigir a la vista de Index después de crear el usuario
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Añadir un error al modelo para que el usuario sepa que hubo un problema
                ModelState.AddModelError(
                    "",
                    "Hubo un error al crear el usuario. Por favor, inténtalo de nuevo."
                );

                // Retornar la vista con el modelo de usuario para que el usuario pueda corregir los errores
                return View(usuario);
            }
        }

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
                        ExpiresUtc = DateTime.UtcNow.AddDays(7),
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
