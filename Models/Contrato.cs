using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models
{
    public class Contrato
    {
        public int IdContrato { get; set; }

        public int IdUsuarioCrea { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int? IdUsuarioAnula { get; set; }
        public DateTime? FechaRescision { get; set; }

        // ¡Aquí faltaba la propiedad FechaUsuarioAnula!
        public DateTime? FechaUsuarioAnula { get; set; }

        public int IdInmueble { get; set; }

        public int IdInquilino { get; set; }

        [Required(ErrorMessage = "La Fecha de Inicio es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La Fecha de Inicio debe ser una fecha válida.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La Fecha de Fin es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La Fecha de Fin debe ser una fecha válida.")]
        public DateTime FechaFin { get; set; }

        public decimal MontoMensual { get; set; }

        public bool Activo { get; set; }

        public string? EstadoContrato { get; set; }

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        // Extras para mostrar en la vista (NO agregar [Required] aquí)
        public string? DireccionInmueble { get; set; }
        public string? NombreInquilino { get; set; }

        // Propiedades para Propietario
        public int IdPropietario { get; set; }
        public string? NombrePropietario { get; set; }

        // Propiedades de Navegación
        public Inmueble? Inmueble { get; set; }
        public Inquilino? Inquilino { get; set; }
        public Propietario? Propietario { get; set; }
    }
}
