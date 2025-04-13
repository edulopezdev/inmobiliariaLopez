using System.Collections.Generic;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorio<T>
    {
        // Obtener todos los elementos
        IList<T> Index();

        // Obtener un elemento por su ID
        T? Details(int id);

        // Crear un nuevo elemento
        int Create(T entidad);

        // Actualizar un elemento existente
        int Edit(T entidad);

        // Eliminar un elemento por su ID
        int Delete(int id);
    }
}
