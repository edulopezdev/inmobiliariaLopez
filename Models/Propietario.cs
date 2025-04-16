using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models
{
    public class Propietario
    {
        public int IdPropietario { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        public required string DNI { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public required string Apellido { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public required string Nombre { get; set; }

        // Propiedad solo para mostrar el nombre completo en la UI
        public string NombreCompleto => $"{Nombre} {Apellido}";

        // El teléfono puede ser nullable
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El email no es válido.")]
        [Required(ErrorMessage = "El email es obligatorio.")]
        public required string Email { get; set; }
    }
}
