using InmobiliariaLopez.Models;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        // Método específico para Propietario
        Propietario? ObtenerPorDNI(string dni);
    }
}