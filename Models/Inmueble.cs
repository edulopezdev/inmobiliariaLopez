using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models
{
    public class Inmueble
    {
        public int IdInmueble { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public required string Direccion { get; set; }

        [Required(ErrorMessage = "El uso es obligatorio.")]
        public required string Uso { get; set; } // 'Comercial' o 'Residencial'

        [Required(ErrorMessage = "El tipo de inmueble es obligatorio.")]
        public int IdTipoInmueble { get; set; }

        [Required(ErrorMessage = "La cantidad de ambientes es obligatoria.")]
        public int Ambientes { get; set; }

        public string? Coordenadas { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public required string Estado { get; set; } // 'Disponible', 'No Disponible', 'Alquilado'

        [Required(ErrorMessage = "El propietario es obligatorio.")]
        public int IdPropietario { get; set; }

        // Nueva propiedad para mostrar nombre del tipo de inmueble
        public string? TipoNombre { get; set; }
        public string? PropietarioApellido { get; set; }
        public string? PropietarioNombre { get; set; }

        // Nueva propiedad para concatenar apellido y nombre del propietario
        public string PropietarioCompleto => $"{PropietarioApellido} {PropietarioNombre}";
    }
}
