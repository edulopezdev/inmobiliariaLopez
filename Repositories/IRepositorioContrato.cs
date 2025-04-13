using System.Collections.Generic;
using InmobiliariaLopez.Models;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        IList<Contrato> ObtenerPorInmueble(int idInmueble);
        IList<Contrato> ObtenerPorInquilino(int idInquilino);
        Contrato? ObtenerPorId(int idContrato);
    }
}
