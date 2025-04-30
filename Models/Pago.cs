using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models
{
    public class Pago
    {
        public int IdPago { get; set; }

        public string UsuarioRegistro { get; set; } = string.Empty;

        public int NumeroPago { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }

        public string TipoPago { get; set; } = string.Empty;

        public int? MesesAdeudados { get; set; }

        public string Estado { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Detalle { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor que 0.")]
        public decimal Importe { get; set; }

        public int NumeroPagoContrato { get; set; }

        public int? IdUsuarioCrea { get; set; }

        public Usuario? UsuarioCrea { get; set; }

        public int? IdUsuarioAnula { get; set; }

        public Usuario? UsuarioAnula { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaAnulacion { get; set; }

        public string? MotivoAnulacion { get; set; }

        public int? IdMulta { get; set; }

        public Multa? Multa { get; set; }

        public int IdContrato { get; set; }
        public Contrato Contrato { get; set; } = new Contrato();

        public bool Activo { get; set; } = true;
    }
}
