using System;
using System.Collections.Generic;
using InmobiliariaLopez.Data;
using InmobiliariaLopez.Models;
using MySql.Data.MySqlClient;

namespace InmobiliariaLopez.Repositories
{
    public class PagoRepository : IRepositorioPago
    {
        private readonly DatabaseConnection _dbConnection;

        public PagoRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Lista todos los Pagos
        public IList<Pago> Index()
        {
            var pagos = new List<Pago>();

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"
                        SELECT IdPago, NumeroPago, FechaPago, TipoPago, MesesAdeudados, Estado, Detalle, Importe,
                               IdUsuarioCrea, IdUsuarioAnula, FechaCreacion, FechaAnulacion, IdMulta, IdContrato, Activo
                        FROM pago WHERE Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pagos.Add(
                                new Pago
                                {
                                    IdPago = reader.GetInt32("IdPago"),
                                    NumeroPago = reader.GetInt32("NumeroPago"),
                                    FechaPago = reader.GetDateTime("FechaPago"),
                                    TipoPago = reader.GetString("TipoPago"),
                                    MesesAdeudados = reader.IsDBNull(
                                        reader.GetOrdinal("MesesAdeudados")
                                    )
                                        ? (int?)null
                                        : reader.GetInt32("MesesAdeudados"),
                                    Estado = reader.GetString("Estado"),
                                    Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle"))
                                        ? null
                                        : reader.GetString("Detalle"),
                                    Importe = reader.GetDecimal("Importe"),
                                    IdUsuarioCrea = reader.IsDBNull(
                                        reader.GetOrdinal("IdUsuarioCrea")
                                    )
                                        ? (int?)null
                                        : reader.GetInt32("IdUsuarioCrea"),
                                    IdUsuarioAnula = reader.IsDBNull(
                                        reader.GetOrdinal("IdUsuarioAnula")
                                    )
                                        ? (int?)null
                                        : reader.GetInt32("IdUsuarioAnula"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                    FechaAnulacion = reader.IsDBNull(
                                        reader.GetOrdinal("FechaAnulacion")
                                    )
                                        ? (DateTime?)null
                                        : reader.GetDateTime("FechaAnulacion"),
                                    IdMulta = reader.IsDBNull(reader.GetOrdinal("IdMulta"))
                                        ? (int?)null
                                        : reader.GetInt32("IdMulta"),
                                    IdContrato = reader.GetInt32("IdContrato"),
                                    Activo = reader.GetBoolean("Activo"),
                                }
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener pagos: {ex.Message}");
                    throw;
                }
            }

            return pagos;
        }

        // Obtiene un Pago por su ID
        public Pago? Details(int id)
        {
            Pago? pago = null;

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"
                        SELECT IdPago, NumeroPago, FechaPago, TipoPago, MesesAdeudados, Estado, Detalle, Importe,
                               IdUsuarioCrea, IdUsuarioAnula, FechaCreacion, FechaAnulacion, IdMulta, IdContrato, Activo
                        FROM pago WHERE IdPago = @IdPago AND Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdPago", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pago = new Pago
                                {
                                    IdPago = reader.GetInt32("IdPago"),
                                    NumeroPago = reader.GetInt32("NumeroPago"),
                                    FechaPago = reader.GetDateTime("FechaPago"),
                                    TipoPago = reader.GetString("TipoPago"),
                                    MesesAdeudados = reader.IsDBNull(
                                        reader.GetOrdinal("MesesAdeudados")
                                    )
                                        ? (int?)null
                                        : reader.GetInt32("MesesAdeudados"),
                                    Estado = reader.GetString("Estado"),
                                    Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle"))
                                        ? null
                                        : reader.GetString("Detalle"),
                                    Importe = reader.GetDecimal("Importe"),
                                    IdUsuarioCrea = reader.IsDBNull(
                                        reader.GetOrdinal("IdUsuarioCrea")
                                    )
                                        ? (int?)null
                                        : reader.GetInt32("IdUsuarioCrea"),
                                    IdUsuarioAnula = reader.IsDBNull(
                                        reader.GetOrdinal("IdUsuarioAnula")
                                    )
                                        ? (int?)null
                                        : reader.GetInt32("IdUsuarioAnula"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                    FechaAnulacion = reader.IsDBNull(
                                        reader.GetOrdinal("FechaAnulacion")
                                    )
                                        ? (DateTime?)null
                                        : reader.GetDateTime("FechaAnulacion"),
                                    IdMulta = reader.IsDBNull(reader.GetOrdinal("IdMulta"))
                                        ? (int?)null
                                        : reader.GetInt32("IdMulta"),
                                    IdContrato = reader.GetInt32("IdContrato"),
                                    Activo = reader.GetBoolean("Activo"),
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener pago por ID: {ex.Message}");
                    throw;
                }
            }

            return pago;
        }

        // Crea un nuevo Pago
        public int Create(Pago entidad)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"
                        INSERT INTO pago (NumeroPago, FechaPago, TipoPago, MesesAdeudados, Estado, Detalle, Importe,
                                          IdUsuarioCrea, FechaCreacion, IdMulta, IdContrato, Activo)
                        VALUES (@NumeroPago, @FechaPago, @TipoPago, @MesesAdeudados, @Estado, @Detalle, @Importe,
                                @IdUsuarioCrea, @FechaCreacion, @IdMulta, @IdContrato, @Activo);
                        SELECT LAST_INSERT_ID();",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@NumeroPago", entidad.NumeroPago);
                        command.Parameters.AddWithValue("@FechaPago", entidad.FechaPago);
                        command.Parameters.AddWithValue("@TipoPago", entidad.TipoPago);
                        command.Parameters.AddWithValue(
                            "@MesesAdeudados",
                            entidad.MesesAdeudados.HasValue
                                ? (object)entidad.MesesAdeudados
                                : DBNull.Value
                        );
                        command.Parameters.AddWithValue("@Estado", entidad.Estado);
                        command.Parameters.AddWithValue(
                            "@Detalle",
                            string.IsNullOrEmpty(entidad.Detalle)
                                ? DBNull.Value
                                : (object)entidad.Detalle
                        );
                        command.Parameters.AddWithValue("@Importe", entidad.Importe);
                        command.Parameters.AddWithValue(
                            "@IdUsuarioCrea",
                            entidad.IdUsuarioCrea.HasValue
                                ? (object)entidad.IdUsuarioCrea
                                : DBNull.Value
                        );
                        command.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                        command.Parameters.AddWithValue(
                            "@IdMulta",
                            entidad.IdMulta.HasValue ? (object)entidad.IdMulta : DBNull.Value
                        );
                        command.Parameters.AddWithValue("@IdContrato", entidad.IdContrato);
                        command.Parameters.AddWithValue("@Activo", 1);

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear pago: {ex.Message}");
                    throw;
                }
            }
        }

        // Actualiza un Pago existente
        public int Edit(Pago entidad)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"
                        UPDATE pago
                        SET NumeroPago = @NumeroPago,
                            FechaPago = @FechaPago,
                            TipoPago = @TipoPago,
                            MesesAdeudados = @MesesAdeudados,
                            Estado = @Estado,
                            Detalle = @Detalle,
                            Importe = @Importe,
                            IdUsuarioCrea = @IdUsuarioCrea,
                            IdUsuarioAnula = @IdUsuarioAnula,
                            FechaAnulacion = @FechaAnulacion,
                            IdMulta = @IdMulta,
                            IdContrato = @IdContrato
                        WHERE IdPago = @IdPago",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdPago", entidad.IdPago);
                        command.Parameters.AddWithValue("@NumeroPago", entidad.NumeroPago);
                        command.Parameters.AddWithValue("@FechaPago", entidad.FechaPago);
                        command.Parameters.AddWithValue("@TipoPago", entidad.TipoPago);
                        command.Parameters.AddWithValue(
                            "@MesesAdeudados",
                            entidad.MesesAdeudados.HasValue
                                ? (object)entidad.MesesAdeudados
                                : DBNull.Value
                        );
                        command.Parameters.AddWithValue("@Estado", entidad.Estado);
                        command.Parameters.AddWithValue(
                            "@Detalle",
                            string.IsNullOrEmpty(entidad.Detalle)
                                ? DBNull.Value
                                : (object)entidad.Detalle
                        );
                        command.Parameters.AddWithValue("@Importe", entidad.Importe);
                        command.Parameters.AddWithValue(
                            "@IdUsuarioCrea",
                            entidad.IdUsuarioCrea.HasValue
                                ? (object)entidad.IdUsuarioCrea
                                : DBNull.Value
                        );
                        command.Parameters.AddWithValue(
                            "@IdUsuarioAnula",
                            entidad.IdUsuarioAnula.HasValue
                                ? (object)entidad.IdUsuarioAnula
                                : DBNull.Value
                        );
                        command.Parameters.AddWithValue(
                            "@FechaAnulacion",
                            entidad.FechaAnulacion.HasValue
                                ? (object)entidad.FechaAnulacion
                                : DBNull.Value
                        );
                        command.Parameters.AddWithValue(
                            "@IdMulta",
                            entidad.IdMulta.HasValue ? (object)entidad.IdMulta : DBNull.Value
                        );
                        command.Parameters.AddWithValue("@IdContrato", entidad.IdContrato);

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al editar pago: {ex.Message}");
                    throw;
                }
            }
        }

        // "Elimina" (desactiva) un Pago por su ID
        public int Delete(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "UPDATE pago SET Activo = 0 WHERE IdPago = @IdPago",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdPago", id);
                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar pago: {ex.Message}");
                    throw;
                }
            }
        }

        // Obtener todos los pagos asociados a un contrato específico
        public IList<Pago> ObtenerPagosPorContrato(int contratoId)
        {
            var pagos = new List<Pago>();

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"
                        SELECT IdPago, NumeroPago, FechaPago, TipoPago, MesesAdeudados, Estado, Detalle, Importe,
                               IdUsuarioCrea, IdUsuarioAnula, FechaCreacion, FechaAnulacion, IdMulta, IdContrato, Activo
                        FROM pago WHERE IdContrato = @IdContrato AND Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdContrato", contratoId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pagos.Add(
                                    new Pago
                                    {
                                        IdPago = reader.GetInt32("IdPago"),
                                        NumeroPago = reader.GetInt32("NumeroPago"),
                                        FechaPago = reader.GetDateTime("FechaPago"),
                                        TipoPago = reader.GetString("TipoPago"),
                                        MesesAdeudados = reader.IsDBNull(
                                            reader.GetOrdinal("MesesAdeudados")
                                        )
                                            ? (int?)null
                                            : reader.GetInt32("MesesAdeudados"),
                                        Estado = reader.GetString("Estado"),
                                        Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle"))
                                            ? null
                                            : reader.GetString("Detalle"),
                                        Importe = reader.GetDecimal("Importe"),
                                        IdUsuarioCrea = reader.IsDBNull(
                                            reader.GetOrdinal("IdUsuarioCrea")
                                        )
                                            ? (int?)null
                                            : reader.GetInt32("IdUsuarioCrea"),
                                        IdUsuarioAnula = reader.IsDBNull(
                                            reader.GetOrdinal("IdUsuarioAnula")
                                        )
                                            ? (int?)null
                                            : reader.GetInt32("IdUsuarioAnula"),
                                        FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                        FechaAnulacion = reader.IsDBNull(
                                            reader.GetOrdinal("FechaAnulacion")
                                        )
                                            ? (DateTime?)null
                                            : reader.GetDateTime("FechaAnulacion"),
                                        IdMulta = reader.IsDBNull(reader.GetOrdinal("IdMulta"))
                                            ? (int?)null
                                            : reader.GetInt32("IdMulta"),
                                        IdContrato = reader.GetInt32("IdContrato"),
                                        Activo = reader.GetBoolean("Activo"),
                                    }
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener pagos por contrato: {ex.Message}");
                    throw;
                }
            }

            return pagos;
        }

        // Obtener el último número de pago para un contrato específico
        public int? ObtenerUltimoNumeroPago(int contratoId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"
                        SELECT MAX(NumeroPago)
                        FROM pago
                        WHERE IdContrato = @IdContrato",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdContrato", contratoId);
                        var result = command.ExecuteScalar();
                        return result == DBNull.Value ? (int?)null : Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener el último número de pago: {ex.Message}");
                    throw;
                }
            }
        }

        // Repositorio: PagoRepository.cs
        public int AnularPago(int pagoId, int usuarioAnulaId, string motivoAnulacion)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"
                    UPDATE pago
                    SET Estado = @Estado,
                        IdUsuarioAnula = @IdUsuarioAnula,
                        FechaAnulacion = @FechaAnulacion,
                        MotivoAnulacion = @MotivoAnulacion
                    WHERE IdPago = @IdPago",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdPago", pagoId);
                        command.Parameters.AddWithValue("@Estado", "Anulado");
                        command.Parameters.AddWithValue("@IdUsuarioAnula", usuarioAnulaId);
                        command.Parameters.AddWithValue("@FechaAnulacion", DateTime.Now);
                        command.Parameters.AddWithValue("@MotivoAnulacion", motivoAnulacion ?? "");

                        int result = command.ExecuteNonQuery();

                        // Log para depuración
                        if (result == 0)
                        {
                            Console.WriteLine($"No se actualizó el pago con ID = {pagoId}");
                            throw new Exception("No se pudo actualizar el pago.");
                        }

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    // Log de error
                    Console.WriteLine($"Error al anular pago: {ex.Message}");
                    throw; // Re-lanzamos el error para que lo capture el controller
                }
            }
        }
    }
}
