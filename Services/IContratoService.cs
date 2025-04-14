using System;
using System.Collections.Generic;
using InmobiliariaLopez.Services; // Asegúrate de tener este using

namespace InmobiliariaLopez.Services
{
    public interface IContratoService
    {
        bool EsInmuebleDisponible(int idInmueble, DateTime fechaInicio, DateTime fechaFin);
        DisponibilidadResultado VerificarDisponibilidad(
            int idInmueble,
            DateTime fechaInicio,
            DateTime fechaFin
        );

        // Agrega esta nueva sobrecarga con el parámetro opcional
        DisponibilidadResultado VerificarDisponibilidad(
            int idInmueble,
            DateTime fechaInicio,
            DateTime fechaFin,
            int? contratoIdExcluir = null
        );

        bool EsInmuebleDisponible(
            int idInmueble,
            DateTime fechaInicio,
            DateTime fechaFin,
            int? contratoIdExcluir = null
        );
    }
}
