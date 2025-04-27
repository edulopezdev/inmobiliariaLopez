using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        public string Email { get; set; }

        public string? ContrasenaHasheada { get; set; }
        public string? NuevaContrasena { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        public string Rol { get; set; } // "Administrador" o "Empleado"
        public string? Avatar { get; set; } // Ruta de imagen
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public bool Activo { get; set; }
    }
}
