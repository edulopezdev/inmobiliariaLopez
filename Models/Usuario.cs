using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Contrasena { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = string.Empty; // Puede ser un enum: Administrador, Empleado

        public string? Avatar { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public bool Activo { get; set; }
    }
}
