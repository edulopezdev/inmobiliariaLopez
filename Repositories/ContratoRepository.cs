using System;
using System.Collections.Generic;
using InmobiliariaLopez.Data;
using InmobiliariaLopez.Models;
using MySql.Data.MySqlClient;

namespace InmobiliariaLopez.Repositories
{
    public class ContratoRepository : IRepositorioContrato
    {
        private readonly DatabaseConnection _dbConnection;

        public ContratoRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Métodos heredados de IRepositorio<T>

        // Lista los contratos
        public IList<Contrato> Index()
        {
            var lista = new List<Contrato>();
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        @"
                    SELECT c.*, i.Direccion AS DireccionInmueble, q.Apellido, q.Nombre
                    FROM contrato c
                    JOIN inmueble i ON c.IdInmueble = i.IdInmueble
                    JOIN inquilino q ON c.IdInquilino = q.IdInquilino
                    WHERE c.Activo = 1 AND i.Activo = 1 AND q.Activo = 1",
                        (MySqlConnection)connection
                    )
                )
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Contrato
                                {
                                    IdContrato = reader.GetInt32("IdContrato"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                    IdUsuarioCrea = reader.GetInt32("IdUsuarioCrea"),
                                    IdInmueble = reader.GetInt32("IdInmueble"),
                                    DireccionInmueble = reader.GetString("DireccionInmueble"),
                                    IdInquilino = reader.GetInt32("IdInquilino"),
                                    NombreInquilino =
                                        $"{reader.GetString("Nombre")} {reader.GetString("Apellido")}",
                                    FechaInicio = reader.GetDateTime("FechaInicio"),
                                    FechaFin = reader.GetDateTime("FechaFin"),
                                    MontoMensual = reader.GetDecimal("MontoMensual"),
                                    Multa = reader.IsDBNull(reader.GetOrdinal("Multa"))
                                        ? null
                                        : reader.GetDecimal("Multa"),
                                }
                            );
                        }
                    }
                }
            }
            return lista;
        }

        // Obtiene un contrato por su ID
        public Contrato? Details(int id)
        {
            Contrato? contrato = null;
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"
                        SELECT c.*, i.Direccion AS DireccionInmueble, q.Apellido AS ApellidoInquilino, q.Nombre AS NombreInquilino,
                               p.IdPropietario, p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario 
                        FROM contrato c
                        JOIN inmueble i ON c.IdInmueble = i.IdInmueble
                        JOIN inquilino q ON c.IdInquilino = q.IdInquilino
                        JOIN propietario p ON i.IdPropietario = p.IdPropietario 
                        WHERE c.IdContrato = @IdContrato AND c.Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdContrato", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                contrato = new Contrato
                                {
                                    IdContrato = reader.GetInt32("IdContrato"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                    IdUsuarioCrea = reader.GetInt32("IdUsuarioCrea"),
                                    IdInmueble = reader.GetInt32("IdInmueble"),
                                    DireccionInmueble = reader.GetString("DireccionInmueble"),
                                    IdInquilino = reader.GetInt32("IdInquilino"),
                                    NombreInquilino =
                                        $"{reader.GetString("NombreInquilino")} {reader.GetString("ApellidoInquilino")}",
                                    FechaInicio = reader.GetDateTime("FechaInicio"),
                                    FechaFin = reader.GetDateTime("FechaFin"),
                                    MontoMensual = reader.GetDecimal("MontoMensual"),
                                    Multa = reader.IsDBNull(reader.GetOrdinal("Multa"))
                                        ? null
                                        : reader.GetDecimal("Multa"),
                                    Activo = reader.GetBoolean("Activo"),
                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    NombrePropietario =
                                        $"{reader.GetString("NombrePropietario")} {reader.GetString("ApellidoPropietario")}",
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener el contrato: " + ex.Message);
                }
            }
            return contrato;
        }

        public int Create(Contrato entidad)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        @"
            INSERT INTO contrato 
            (IdUsuarioCrea, FechaCreacion, IdInmueble, IdInquilino, FechaInicio, FechaFin, MontoMensual, Multa, Activo, EstadoContrato)
            VALUES 
            (@IdUsuarioCrea, @FechaCreacion, @IdInmueble, @IdInquilino, @FechaInicio, @FechaFin, @MontoMensual, @Multa, 1, @EstadoContrato)",
                        (MySqlConnection)connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@IdUsuarioCrea", entidad.IdUsuarioCrea);
                    command.Parameters.AddWithValue("@FechaCreacion", entidad.FechaCreacion);
                    command.Parameters.AddWithValue("@IdInmueble", entidad.IdInmueble);
                    command.Parameters.AddWithValue("@IdInquilino", entidad.IdInquilino);
                    command.Parameters.AddWithValue("@FechaInicio", entidad.FechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", entidad.FechaFin);
                    command.Parameters.AddWithValue("@MontoMensual", entidad.MontoMensual);
                    command.Parameters.AddWithValue(
                        "@Multa",
                        entidad.Multa ?? (object)DBNull.Value
                    );
                    command.Parameters.AddWithValue(
                        "@EstadoContrato",
                        string.IsNullOrEmpty(entidad.EstadoContrato)
                            ? "Vigente"
                            : entidad.EstadoContrato
                    );

                    command.ExecuteNonQuery();
                    return (int)command.LastInsertedId;
                }
            }
        }

        public int Edit(Contrato entidad)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();

                // Cambiar INSERT por UPDATE para la edición del contrato
                using (
                    var command = new MySqlCommand(
                        @"
                UPDATE contrato
                SET
                    FechaInicio = IFNULL(@FechaInicio, FechaInicio),
                    FechaFin = IFNULL(@FechaFin, FechaFin),
                    MontoMensual = IFNULL(@MontoMensual, MontoMensual),
                    Multa = IFNULL(@Multa, Multa),
                    EstadoContrato = IFNULL(@EstadoContrato, EstadoContrato),
                    Observaciones = IFNULL(@Observaciones, Observaciones)
                WHERE IdContrato = @IdContrato;",
                        (MySqlConnection)connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@IdContrato", entidad.IdContrato);
                    command.Parameters.AddWithValue("@FechaInicio", entidad.FechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", entidad.FechaFin);
                    command.Parameters.AddWithValue("@MontoMensual", entidad.MontoMensual);
                    command.Parameters.AddWithValue(
                        "@Multa",
                        entidad.Multa ?? (object)DBNull.Value
                    );
                    command.Parameters.AddWithValue("@EstadoContrato", entidad.EstadoContrato);
                    command.Parameters.AddWithValue(
                        "@Observaciones",
                        entidad.Observaciones ?? (object)DBNull.Value
                    );

                    return command.ExecuteNonQuery();
                }
            }
        }

        public int Delete(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        @"
                    UPDATE contrato SET Activo = 0 WHERE IdContrato = @IdContrato",
                        (MySqlConnection)connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@IdContrato", id);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public IList<Contrato> ObtenerPorInmueble(int idInmueble)
        {
            var lista = new List<Contrato>();
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        @"
                    SELECT * FROM contrato WHERE IdInmueble = @IdInmueble AND Activo = 1",
                        (MySqlConnection)connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@IdInmueble", idInmueble);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Contrato
                                {
                                    IdContrato = reader.GetInt32("IdContrato"),
                                    IdInmueble = reader.GetInt32("IdInmueble"),
                                    IdInquilino = reader.GetInt32("IdInquilino"),
                                    FechaInicio = reader.GetDateTime("FechaInicio"),
                                    FechaFin = reader.GetDateTime("FechaFin"),
                                    MontoMensual = reader.GetDecimal("MontoMensual"),
                                    Multa = reader.IsDBNull(reader.GetOrdinal("Multa"))
                                        ? null
                                        : reader.GetDecimal("Multa"),
                                }
                            );
                        }
                    }
                }
            }
            return lista;
        }

        public IList<Contrato> ObtenerPorInquilino(int idInquilino)
        {
            var lista = new List<Contrato>();
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        @"
                    SELECT * FROM contrato WHERE IdInquilino = @IdInquilino AND Activo = 1",
                        (MySqlConnection)connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@IdInquilino", idInquilino);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Contrato
                                {
                                    IdContrato = reader.GetInt32("IdContrato"),
                                    IdInmueble = reader.GetInt32("IdInmueble"),
                                    IdInquilino = reader.GetInt32("IdInquilino"),
                                    FechaInicio = reader.GetDateTime("FechaInicio"),
                                    FechaFin = reader.GetDateTime("FechaFin"),
                                    MontoMensual = reader.GetDecimal("MontoMensual"),
                                    Multa = reader.IsDBNull(reader.GetOrdinal("Multa"))
                                        ? null
                                        : reader.GetDecimal("Multa"),
                                }
                            );
                        }
                    }
                }
            }
            return lista;
        }

        public Contrato? ObtenerPorId(int id)
        {
            Contrato? contrato = null;
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        @"
            SELECT c.*, i.Direccion AS DireccionInmueble, q.Apellido AS ApellidoInquilino, q.Nombre AS NombreInquilino,
                   p.IdPropietario, p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario 
            FROM contrato c
            JOIN inmueble i ON c.IdInmueble = i.IdInmueble
            JOIN inquilino q ON c.IdInquilino = q.IdInquilino
            JOIN propietario p ON i.IdPropietario = p.IdPropietario 
            WHERE c.IdContrato = @IdContrato AND c.Activo = 1",
                        (MySqlConnection)connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@IdContrato", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32("IdContrato"),
                                FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                IdUsuarioCrea = reader.GetInt32("IdUsuarioCrea"),
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                DireccionInmueble = reader.GetString("DireccionInmueble"),
                                IdInquilino = reader.GetInt32("IdInquilino"),
                                NombreInquilino =
                                    $"{reader.GetString("NombreInquilino")} {reader.GetString("ApellidoInquilino")}",
                                FechaInicio = reader.GetDateTime("FechaInicio"),
                                FechaFin = reader.GetDateTime("FechaFin"),
                                MontoMensual = reader.GetDecimal("MontoMensual"),
                                Multa = reader.IsDBNull(reader.GetOrdinal("Multa"))
                                    ? null
                                    : reader.GetDecimal("Multa"),
                                Activo = reader.GetBoolean("Activo"),
                                IdPropietario = reader.GetInt32("IdPropietario"),
                                NombrePropietario =
                                    $"{reader.GetString("NombrePropietario")} {reader.GetString("ApellidoPropietario")}",
                            };
                        }
                    }
                }
            }
            return contrato;
        }
    }
}
