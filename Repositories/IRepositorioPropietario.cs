using InmobiliariaLopez.Models;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        // Método específico para Propietario
        Propietario? ObtenerPorDNI(string dni);

        // Método para obtener los propietarios con filtros (DNI, Apellido, Nombre)
        IList<Propietario> ObtenerPorFiltro(
            string dni,
            string apellido,
            string nombre,
            int pagina,
            int cantidadPorPagina
        );

        // Método para obtener el total de propietarios con filtros
        int ObtenerTotalPorFiltro(string dni, string apellido, string nombre);
    }
}
