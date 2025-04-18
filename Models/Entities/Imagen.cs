using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models;

public class Imagen
{
    public int IdImagen { get; set; }
    public string? Ruta { get; set; } // Ej: "/uploads/inmuebles/portada_123.jpg"
    public string? TipoImagen { get; set; } // "InmueblePortada", "InmuebleInterior", "AvatarUsuario"
    public int IdRelacionado { get; set; } // ID del inmueble/usuario
    public bool Activo { get; set; } = true;
}
