using InmobiliariaLopez.Models;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorioInquilino : IRepositorio<Inquilino>
    {
        // Método específico para Inquilino
        Inquilino? ObtenerPorDNI(string dni);
        IList<Inquilino> ObtenerPorFiltro(
            string dni,
            string apellido,
            string nombre,
            int pagina,
            int cantidadPorPagina
        );
        int ObtenerTotalPorFiltro(string dni, string apellido, string nombre);
    }
}
