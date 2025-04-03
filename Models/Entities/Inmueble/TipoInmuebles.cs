namespace InmobiliariaLopez.Models
{
    public class TipoInmueble
    {
        public int IdTipoInmueble { get; set; }  // ID único para el tipo de inmueble
        public required string Nombre { get; set; }       // Nombre del tipo de inmueble (por ejemplo, Casa, Departamento)
    }
}
