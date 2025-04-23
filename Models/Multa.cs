using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models
{
    public class Multa
    {
        public int IdMulta { get; set; }

        public int IdContrato { get; set; }
        public Contrato Contrato { get; set; } // Propiedad de navegación

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto de la multa debe ser mayor que 0.")]
        public decimal Monto { get; set; }

        public DateTime FechaCalculo { get; set; }

        [MaxLength(500)]
        public string Motivo { get; set; }

        public bool Pagada { get; set; }
    }
}
