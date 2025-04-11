using System;
using System.ComponentModel.DataAnnotations; // Importa para los atributos de validación (opcional)

namespace InmobiliariaLopez.Models
{
    public class Contrato
    {
        public int IdContrato { get; set; }
        public int IdUsuarioCrea { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? IdUsuarioAnula { get; set; }
        public DateTime? FechaRescision { get; set; }
        public int IdInmueble { get; set; }
        public int IdInquilino { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal MontoMensual { get; set; }
        public decimal? Multa { get; set; }
        public bool Activo { get; set; }

        // Extras para mostrar en la vista
        public string? DireccionInmueble { get; set; }
        public string? NombreInquilino { get; set; }

        // Propiedades para Propietario
        public int IdPropietario { get; set; } // Clave externa al Propietario
        public string? NombrePropietario { get; set; } // Para mostrar el nombre en la vista

        // Propiedades de Navegación (Opcional, si usas Entity Framework)
        // Estas propiedades permiten a Entity Framework 
        // cargar automáticamente las entidades relacionadas.
        public Inmueble? Inmueble { get; set; }
        public Inquilino? Inquilino { get; set; }
        public Propietario? Propietario { get; set; }
    }
}