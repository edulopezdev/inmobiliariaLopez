using InmobiliariaLopez.Models;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        // Método específico: Obtener inmuebles por propietario
        IList<Inmueble> ObtenerPorPropietario(int idPropietario);

        // Método específico: Verificar disponibilidad de un inmueble en un rango de fechas
        bool EstaDisponibleEnFechas(int idInmueble, DateTime fechaInicio, DateTime fechaFin);
    }
}