using System;
using System.Collections.Generic;
using InmobiliariaLopez.Data;
using InmobiliariaLopez.Repositories;
using MySql.Data.MySqlClient;

namespace InmobiliariaLopez.Services
{
    public class DisponibilidadResultado
    {
        public bool Disponible { get; set; }
        public List<Tuple<DateTime, DateTime>> FechasOcupadas { get; set; } = new();
    }

    public class ContratoService : IContratoService
    {
        private readonly DatabaseConnection _dbConnection;
        private readonly IRepositorioContrato _repositorioContrato;

        public ContratoService(
            DatabaseConnection dbConnection,
            IRepositorioContrato repositorioContrato
        )
        {
            _dbConnection = dbConnection;
            _repositorioContrato = repositorioContrato;
        }

        public bool EsInmuebleDisponible(int idInmueble, DateTime fechaInicio, DateTime fechaFin)
        {
            return !VerificarDisponibilidad(idInmueble, fechaInicio, fechaFin).Disponible;
        }

        public DisponibilidadResultado VerificarDisponibilidad(
            int idInmueble,
            DateTime fechaInicio,
            DateTime fechaFin
        )
        {
            var resultado = new DisponibilidadResultado { Disponible = true };

            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                string sql =
                    @"
                    SELECT FechaInicio, FechaFin
                    FROM contrato
                    WHERE IdInmueble = @IdInmueble
                    AND Activo = 1
                    AND (
                        (@FechaInicio BETWEEN FechaInicio AND FechaFin)
                        OR (@FechaFin BETWEEN FechaInicio AND FechaFin)
                        OR (FechaInicio BETWEEN @FechaInicio AND @FechaFin)
                        OR (FechaFin BETWEEN @FechaInicio AND @FechaFin)
                    );";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdInmueble", idInmueble);
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultado.Disponible = false;
                            DateTime inicio = reader.GetDateTime("FechaInicio");
                            DateTime fin = reader.GetDateTime("FechaFin");
                            resultado.FechasOcupadas.Add(Tuple.Create(inicio, fin));
                        }
                    }
                }
            }

            return resultado;
        }
    }
}
