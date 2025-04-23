using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models
{
    public class Pago
    {
        public int IdPago { get; set; }

        public string UsuarioRegistro { get; set; } = string.Empty; // Inicialización a cadena vacía si no puede ser nulo

        public int NumeroPago { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }

        // Declarar como nullable si se espera que pueda ser null
        public string TipoPago { get; set; } = string.Empty; // Evita CS8618

        public int? MesesAdeudados { get; set; }

        public string Estado { get; set; } = string.Empty; // Evita CS8618

        [MaxLength(255)]
        public string Detalle { get; set; } = string.Empty; // Evita CS8618

        [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor que 0.")]
        public decimal Importe { get; set; }

        public int NumeroPagoContrato { get; set; }

        // IdUsuarioCrea puede ser nullable, así que inicializamos como null
        public int? IdUsuarioCrea { get; set; }

        // UsuarioCrea es una propiedad de navegación, inicialización opcional
        public Usuario? UsuarioCrea { get; set; }

        // IdUsuarioAnula puede ser nullable
        public int? IdUsuarioAnula { get; set; }

        // UsuarioAnula es una propiedad de navegación, inicialización opcional
        public Usuario? UsuarioAnula { get; set; }

        // Asegurarse de que FechaCreacion siempre tenga un valor
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // FechaAnulacion es nullable
        public DateTime? FechaAnulacion { get; set; }

        // MotivoAnulacion es nullable
        public string? MotivoAnulacion { get; set; }

        public int? IdMulta { get; set; }

        // Multa es una propiedad de navegación, inicialización opcional
        public Multa? Multa { get; set; }

        // Contrato es una propiedad de navegación, inicialización opcional
        public int IdContrato { get; set; }
        public Contrato Contrato { get; set; } = new Contrato(); // Asegura que la propiedad Contrato no sea null

        // Activo no debería ser null, se asigna por defecto
        public bool Activo { get; set; } = true;
    }
}
