using System.Collections.Generic;
using InmobiliariaLopez.Models;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        // Obtener todos los pagos asociados a un contrato específico
        IList<Pago> ObtenerPagosPorContrato(int contratoId);

        // Obtener el último número de pago para un contrato específico
        int? ObtenerUltimoNumeroPago(int contratoId);

        // Cambiar el estado de un pago a "Anulado"
        // El motivo de la anulación no debe ser nulo y debe ser proporcionado
        int AnularPago(int pagoId, int usuarioAnulaId, string motivoAnulacion);

        IEnumerable<Multa> ObtenerMultasSinPagar();
    }
}
