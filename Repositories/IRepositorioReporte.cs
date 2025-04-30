using System.Collections.Generic;
using InmobiliariaLopez.Models;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorioReporte
    {
        IList<InmuebleConPropietarioViewModel> ObtenerInmueblesConPropietarios(
            bool soloDisponibles
        );
        IList<ContratoViewModel> ObtenerContratosVigentes();
        IList<InmuebleDesocupadoViewModel> ObtenerInmueblesDesocupados(
            DateTime desde,
            DateTime hasta
        );
    }
}
