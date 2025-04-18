using System.ComponentModel.DataAnnotations;

namespace InmobiliariaLopez.Models;

public class Inmueble
{
    public int IdInmueble { get; set; }
    public string? Direccion { get; set; }
    public string? Uso { get; set; }
    public int IdTipoInmueble { get; set; }
    public string? TipoNombre { get; set; }
    public int Ambientes { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public decimal Precio { get; set; }
    public string? Estado { get; set; }
    public int IdPropietario { get; set; }

    // Propiedades adicionales para el propietario
    public string? PropietarioApellido { get; set; }
    public string? PropietarioNombre { get; set; }

    // Nueva propiedad para concatenar apellido y nombre del propietario
    public string PropietarioCompleto =>
        $"{PropietarioApellido ?? ""} {PropietarioNombre ?? ""}".Trim();

    // Propiedades de Navegación
    public Propietario? Propietario { get; set; }

    // Propiedades de Navegación para Imagen
    public List<Imagen> Imagenes { get; set; } = new List<Imagen>();

    // Constructor
    public Inmueble()
    {
        // Valores predeterminados o inicialización vacía
        Direccion = string.Empty;
        Uso = string.Empty;
        Estado = string.Empty;
    }

    // Constructor con tres argumentos
    public Inmueble(string direccion, string uso, string estado)
    {
        Direccion = direccion;
        Uso = uso;
        Estado = estado;
    }
}
