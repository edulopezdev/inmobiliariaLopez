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
        private readonly int _registrosPorPagina = 10;

        public ContratoRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Métodos heredados de IRepositorio<T>

        // Lista los contratos con paginación
        public IList<Contrato> Index(int pagina = 1)
        {
            var lista = new List<Contrato>();
            int skipAmount = (pagina - 1) * _registrosPorPagina;

            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = (MySqlConnection)connection;
                    command.CommandText =
                        @"
                        SELECT
                            c.*,
                            i.Direccion AS DireccionInmueble,
                            CONCAT(q.Nombre, ' ', q.Apellido) AS NombreInquilino,
                            c.FechaRescision
                        FROM contrato c
                        JOIN inmueble i ON c.IdInmueble = i.IdInmueble
                        JOIN inquilino q ON c.IdInquilino = q.IdInquilino
                        WHERE (c.Activo = 1 OR c.EstadoContrato = 'Anulado')
                          AND i.Activo = 1
                          AND q.Activo = 1
                          AND c.EstadoContrato IN ('Vigente', 'PendienteAnulacion', 'Anulado', 'Finalizado')
                        LIMIT @limit OFFSET @offset;";
                    command.Parameters.AddWithValue("@limit", _registrosPorPagina);
                    command.Parameters.AddWithValue("@offset", skipAmount);

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
                                    NombreInquilino = reader.GetString("NombreInquilino"),
                                    FechaInicio = reader.GetDateTime("FechaInicio"),
                                    FechaFin = reader.GetDateTime("FechaFin"),
                                    MontoMensual = reader.GetDecimal("MontoMensual"),
                                    EstadoContrato = reader.GetString("EstadoContrato"),
                                    Activo = reader.GetBoolean("Activo"),
                                    FechaRescision = reader.IsDBNull(
                                        reader.GetOrdinal("FechaRescision")
                                    )
                                        ? null
                                        : reader.GetDateTime("FechaRescision"),
                                }
                            );
                        }
                    }
                }
            }
            return lista;
        }

        // Obtiene la cantidad total de contratos activos
        public int ObtenerTotal()
        {
            int totalRegistros = 0;
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        "SELECT COUNT(*) FROM contrato WHERE Activo = 1",
                        (MySqlConnection)connection
                    )
                )
                {
                    totalRegistros = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return totalRegistros;
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
                                    FechaRescision = reader.IsDBNull(
                                        reader.GetOrdinal("FechaRescision")
                                    )
                                        ? null
                                        : reader.GetDateTime("FechaRescision"),
                                    Activo = reader.GetBoolean("Activo"),
                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    NombrePropietario =
                                        $"{reader.GetString("NombrePropietario")} {reader.GetString("ApellidoPropietario")}",
                                    EstadoContrato = reader.GetString("EstadoContrato"),
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
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
                (IdUsuarioCrea, FechaCreacion, IdInmueble, IdInquilino, FechaInicio, FechaFin, MontoMensual, Activo, EstadoContrato)
                VALUES 
                (@IdUsuarioCrea, @FechaCreacion, @IdInmueble, @IdInquilino, @FechaInicio, @FechaFin, @MontoMensual, 1, @EstadoContrato)",
                        (MySqlConnection)connection
                    )
                )
                {
                    if (entidad.FechaCreacion == default(DateTime))
                    {
                        entidad.FechaCreacion = DateTime.Now;
                    }
                    else
                    {
                        entidad.FechaCreacion = entidad.FechaCreacion.ToLocalTime();
                    }

                    command.Parameters.AddWithValue("@IdUsuarioCrea", entidad.IdUsuarioCrea);
                    command.Parameters.AddWithValue("@FechaCreacion", entidad.FechaCreacion);
                    command.Parameters.AddWithValue("@IdInmueble", entidad.IdInmueble);
                    command.Parameters.AddWithValue("@IdInquilino", entidad.IdInquilino);
                    command.Parameters.AddWithValue("@FechaInicio", entidad.FechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", entidad.FechaFin);
                    command.Parameters.AddWithValue("@MontoMensual", entidad.MontoMensual);
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

                using (
                    var command = new MySqlCommand(
                        @"
                UPDATE contrato
                SET
                    FechaInicio = IFNULL(@FechaInicio, FechaInicio),
                    FechaFin = IFNULL(@FechaFin, FechaFin),
                    MontoMensual = IFNULL(@MontoMensual, MontoMensual),
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

        public int AnularContrato(Contrato entidad)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();

                using (
                    var command = new MySqlCommand(
                        @"
                UPDATE contrato
                SET
                    FechaRescision = @FechaRescision,
                    IdUsuarioAnula = @IdUsuarioAnula,
                    FechaUsuarioAnula = @FechaUsuarioAnula,
                    Observaciones = @Observaciones,
                    EstadoContrato = @EstadoContrato,
                    Activo = @Activo
                WHERE IdContrato = @IdContrato;
                ",
                        (MySqlConnection)connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@IdContrato", entidad.IdContrato);
                    command.Parameters.AddWithValue(
                        "@FechaRescision",
                        entidad.FechaRescision ?? (object)DBNull.Value
                    );
                    command.Parameters.AddWithValue(
                        "@IdUsuarioAnula",
                        entidad.IdUsuarioAnula ?? (object)DBNull.Value
                    );
                    command.Parameters.AddWithValue(
                        "@FechaUsuarioAnula",
                        entidad.FechaUsuarioAnula ?? (object)DBNull.Value
                    );
                    command.Parameters.AddWithValue(
                        "@Observaciones",
                        entidad.Observaciones ?? (object)DBNull.Value
                    );
                    command.Parameters.AddWithValue("@EstadoContrato", entidad.EstadoContrato); // Aquí se pasa 'PendienteAnulacion'
                    command.Parameters.AddWithValue("@Activo", entidad.Activo);

                    return command.ExecuteNonQuery();
                }
            }
        }

        public List<(DateTime, DateTime)> ControlFechas(
            int idInmueble,
            DateTime fechaInicio,
            DateTime fechaFin,
            int? idContratoExcluido = null
        )
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();

                // Consulta SQL para verificar si existe algún contrato activo en las fechas solicitadas
                var query =
                    @"
                SELECT FechaInicio, FechaFin 
                FROM contrato 
                WHERE IdInmueble = @IdInmueble 
                AND Activo = 1
                AND ((FechaInicio <= @FechaFin AND FechaFin >= @FechaInicio) OR 
                     (FechaInicio >= @FechaInicio AND FechaFin <= @FechaFin) OR 
                     (FechaInicio <= @FechaInicio AND FechaFin >= @FechaFin) OR 
                     (FechaInicio <= @FechaFin AND FechaFin >= @FechaFin))";

                // Si se ha pasado un idContratoExcluido, excluimos ese contrato
                if (idContratoExcluido.HasValue)
                {
                    query += " AND IdContrato != @IdContratoExcluido";
                }

                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@IdInmueble", idInmueble);
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);

                    if (idContratoExcluido.HasValue)
                    {
                        command.Parameters.AddWithValue(
                            "@IdContratoExcluido",
                            idContratoExcluido.Value
                        );
                    }

                    var reader = command.ExecuteReader();

                    var fechasOcupadas = new List<(DateTime, DateTime)>();
                    while (reader.Read())
                    {
                        var fechaInicioContrato = reader.GetDateTime(0);
                        var fechaFinContrato = reader.GetDateTime(1);
                        fechasOcupadas.Add((fechaInicioContrato, fechaFinContrato));
                    }

                    return fechasOcupadas;
                }
            }
        }
    }
}
