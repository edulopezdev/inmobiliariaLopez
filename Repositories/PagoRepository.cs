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
        private readonly int _registrosPorPagina = 10;

        public PagoRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Lista todos los Pagos con paginación
        public IList<Pago> Index(int pagina = 1)
        {
            var pagos = new List<Pago>();
            int skipAmount = (pagina - 1) * _registrosPorPagina;

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"
                            SELECT IdPago, NumeroPagoContrato, FechaPago, TipoPago, MesesAdeudados, Estado, Detalle, Importe,
                                   IdUsuarioCrea, IdUsuarioAnula, FechaCreacion, FechaAnulacion, IdMulta, IdContrato, Activo
                            FROM pago
                            WHERE Activo = 1
                            LIMIT @limit OFFSET @offset",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@limit", _registrosPorPagina);
                        command.Parameters.AddWithValue("@offset", skipAmount);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pagos.Add(
                                    new Pago
                                    {
                                        IdPago = reader.GetInt32("IdPago"),
                                        NumeroPagoContrato = reader.GetInt32("NumeroPagoContrato"),
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
                    throw;
                }
            }

            return pagos;
        }

        // Obtiene la cantidad total de pagos activos
        public int ObtenerTotal()
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT COUNT(*) FROM pago WHERE Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
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
                        SELECT IdPago, NumeroPagoContrato, FechaPago, TipoPago, MesesAdeudados, Estado, Detalle, Importe,
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
                                    NumeroPagoContrato = reader.GetInt32("NumeroPagoContrato"),
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
                    throw;
                }
            }

            return pago;
        }

        public int Create(Pago entidad)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();

                    // Paso 1: Obtener el último número de pago registrado para este contrato
                    int? ultimoNumeroPago = ObtenerUltimoNumeroPago(entidad.IdContrato);

                    // Si el último número de pago es null, significa que es el primer pago, entonces lo inicializamos en 0
                    int nuevoNumeroPago = (ultimoNumeroPago ?? 0) + 1;

                    int nuevoIdPago;

                    // Inicia la transacción para asegurar la atomicidad
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Paso 2: Insertar el nuevo pago
                            using (
                                var command = new MySqlCommand(
                                    @"
                        INSERT INTO pago (NumeroPagoContrato, FechaPago, TipoPago, MesesAdeudados, Estado, Detalle, Importe,
                                          IdUsuarioCrea, FechaCreacion, IdMulta, IdContrato, Activo)
                        VALUES (@NumeroPagoContrato, @FechaPago, @TipoPago, @MesesAdeudados, @Estado, @Detalle, @Importe,
                                @IdUsuarioCrea, @FechaCreacion, @IdMulta, @IdContrato, @Activo);
                        SELECT LAST_INSERT_ID();",
                                    (MySqlConnection)connection
                                )
                            )
                            {
                                command.Parameters.AddWithValue(
                                    "@NumeroPagoContrato",
                                    nuevoNumeroPago
                                );
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
                                    entidad.IdMulta.HasValue
                                        ? (object)entidad.IdMulta
                                        : DBNull.Value
                                );
                                command.Parameters.AddWithValue("@IdContrato", entidad.IdContrato);
                                command.Parameters.AddWithValue("@Activo", 1);

                                nuevoIdPago = Convert.ToInt32(command.ExecuteScalar());
                            }

                            // 🔁 Si es un pago de multa, marcamos esa multa como pagada
                            if (entidad.IdMulta.HasValue)
                            {
                                using (
                                    var updateCommand = new MySqlCommand(
                                        "UPDATE Multa SET Pagada = 1 WHERE IdMulta = @idMulta",
                                        (MySqlConnection)connection
                                    )
                                )
                                {
                                    updateCommand.Parameters.AddWithValue(
                                        "@idMulta",
                                        entidad.IdMulta.Value
                                    );
                                    updateCommand.ExecuteNonQuery();
                                }
                            }

                            // Paso 3: Confirmamos la transacción
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            // Si hay algún error, revertimos la transacción
                            transaction.Rollback();
                            throw;
                        }
                    }

                    return nuevoIdPago;
                }
                catch (Exception ex)
                {
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
                        SET NumeroPagoContrato = @NumeroPagoContrato,
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
                        command.Parameters.AddWithValue(
                            "@NumeroPagoContrato",
                            entidad.NumeroPagoContrato
                        );
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
                    throw;
                }
            }
        }

        // "Elimina" (desactiva) un Pago por su ID
        public int Delete(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            try
            {
                connection.Open();
                using var command = new MySqlCommand(
                    "UPDATE pago SET Activo = 0 WHERE IdPago = @IdPago", // Solo actualiza si el pago está activo
                    (MySqlConnection)connection
                );
                command.Parameters.AddWithValue("@IdPago", id);

                var filasAfectadas = command.ExecuteNonQuery();

                return filasAfectadas; // Asegúrate de que retorna filas afectadas
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
                throw;
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
                    SELECT IdPago, NumeroPagoContrato, FechaPago, TipoPago, MesesAdeudados, Estado, Detalle, Importe,
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
                                        NumeroPagoContrato = reader.GetInt32("NumeroPagoContrato"), // Actualizado aquí
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
                        SELECT MAX(NumeroPagoContrato)
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
                            throw new Exception("No se pudo actualizar el pago.");
                        }

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    throw; // Re-lanzamos el error para que lo capture el controller
                }
            }
        }

        // Método que obtiene todas las multas sin pagar
        public IEnumerable<Multa> ObtenerMultasSinPagar()
        {
            // Lógica para consultar las multas no pagadas
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"
                        SELECT IdMulta, IdContrato, Motivo, Monto
                        FROM Multa
                        WHERE Pagada = 0", // Aquí estamos buscando las multas no pagadas
                            (MySqlConnection)connection
                        )
                    )
                    using (var reader = command.ExecuteReader())
                    {
                        var multas = new List<Multa>();
                        while (reader.Read())
                        {
                            multas.Add(
                                new Multa
                                {
                                    IdMulta = reader.GetInt32("IdMulta"),
                                    IdContrato = reader.GetInt32("IdContrato"),
                                    Motivo = reader.GetString("Motivo"),
                                    Monto = reader.GetDecimal("Monto"),
                                }
                            );
                        }
                        return multas;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
