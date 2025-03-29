using InmobiliariaLopez.Models;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorioInquilino : IRepositorio<Inquilino>
    {
        // Método específico para Inquilino
        Inquilino? ObtenerPorDNI(string dni);
    }
}