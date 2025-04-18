using System.Collections.Generic;
using InmobiliariaLopez.Models;

namespace InmobiliariaLopez.Repositories
{
    public interface IRepositorioImagen : IRepositorio<Imagen>
    {
        // Método para reemplazar (o actualizar) la información de una imagen
        int Reemplazar(Imagen imagen);

        // Método para obtener una imagen por su IdImagen
        Imagen? ObtenerPorId(int id);

        // Método para obtener todas las imágenes relacionadas con un Inmueble (IdRelacionado)
        IList<Imagen> ObtenerPorInmueble(int idInmueble);
    }
}
