using System;

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
    }
}
