using System.Collections.Generic;
using System.ComponentModel;

namespace InmobiliariaLopez.Models
{
    public class ReportesViewModel
    {
        public IList<InmuebleConPropietarioViewModel> InmueblesConPropietarios { get; set; }
        public IList<ContratoViewModel> ContratosVigentes { get; set; }
        public IList<InmuebleDesocupadoViewModel> InmueblesDesocupados { get; set; }

        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }

    public class InmuebleConPropietarioViewModel
    {
        public int IdInmueble { get; set; }
        public string Direccion { get; set; }
        public string Uso { get; set; }
        public string TipoInmueble { get; set; }
        public int Ambientes { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }
        public string PropietarioNombre { get; set; }
        public string PropietarioApellido { get; set; }
        public string PropietarioEmail { get; set; }
        public string PropietarioTelefono { get; set; }
    }

    public class ContratoViewModel
    {
        public int IdContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string InquilinoNombre { get; set; }
        public string InmuebleDireccion { get; set; }
    }

    public class InmuebleDesocupadoViewModel
    {
        public int IdInmueble { get; set; }
        public string Direccion { get; set; }
        public string TipoInmueble { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaDesocupado { get; set; }
    }
}
