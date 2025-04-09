using InmobiliariaLopez.Models;
using System.Collections.Generic;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        IList<Contrato> ObtenerPorInmueble(int idInmueble);
        IList<Contrato> ObtenerPorInquilino(int idInquilino);
    }
}
