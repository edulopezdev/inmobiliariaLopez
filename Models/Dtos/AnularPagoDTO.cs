using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models.Dtos
{
    public class AnularPagoDTO
    {
        public int IdPago { get; set; }
        public string? MotivoAnulacion { get; set; }
        public DateTime FechaAnulacion { get; set; }
        public int IdUsuarioAnula { get; set; }
    }
}
