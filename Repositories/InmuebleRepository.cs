using System;
using System.Collections.Generic;
using InmobiliariaLopez.Data;
using InmobiliariaLopez.Models;
using MySql.Data.MySqlClient;

namespace InmobiliariaLopez.Repositories
{
    public class InmuebleRepository : IRepositorioInmueble
    {
        private readonly DatabaseConnection _dbConnection;
        private readonly int _registrosPorPagina = 10;

        public InmuebleRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Propiedad pública para acceder a la cantidad de registros por página
        public int RegistrosPorPagina => _registrosPorPagina;

        // Lista todos los Inmuebles con paginación
        public IList<Inmueble> Index(int pagina = 1)
        {
            var inmuebles = new List<Inmueble>();
            int skipAmount = (pagina - 1) * _registrosPorPagina;

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"SELECT i.*, t.Nombre AS TipoNombre, p.Apellido AS PropietarioApellido, p.Nombre AS PropietarioNombre
                            FROM inmueble i
                            JOIN tipoinmueble t ON i.IdTipoInmueble = t.IdTipoInmueble
                            JOIN propietario p ON i.IdPropietario = p.IdPropietario
                            WHERE i.Activo = 1 AND p.Activo = 1
                            LIMIT @limit OFFSET @offset;",
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
                                inmuebles.Add(
                                    new Inmueble(
                                        reader.GetString("Direccion"),
                                        reader.GetString("Uso"),
                                        reader.GetString("Estado")
                                    )
                                    {
                                        IdInmueble = reader.GetInt32("IdInmueble"),
                                        IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                        TipoNombre = reader.GetString("TipoNombre"),
                                        Ambientes = reader.GetInt32("Ambientes"),
                                        Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader.GetDouble("Latitud")),
                                        Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader.GetDouble("Longitud")),
                                        Precio = reader.GetDecimal("Precio"),
                                        IdPropietario = reader.GetInt32("IdPropietario"),
                                        PropietarioApellido = reader.GetString(
                                            "PropietarioApellido"
                                        ),
                                        PropietarioNombre = reader.GetString("PropietarioNombre"),
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
            return inmuebles;
        }

        // Obtiene la cantidad total de inmuebles activos
        public int ObtenerTotal()
        {
            int totalRegistros = 0;
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT COUNT(*) FROM inmueble WHERE Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        totalRegistros = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return totalRegistros;
        }

        // Obtiene un Inmueble por su ID
        public Inmueble? Details(int id)
        {
            Inmueble? inmueble = null;
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"SELECT i.*, t.Nombre AS TipoNombre, p.Apellido AS PropietarioApellido, p.Nombre AS PropietarioNombre
                            FROM inmueble i
                            JOIN tipoinmueble t ON i.IdTipoInmueble = t.IdTipoInmueble
                            JOIN propietario p ON i.IdPropietario = p.IdPropietario
                            WHERE i.IdInmueble = @IdInmueble AND i.Activo = 1 AND p.Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdInmueble", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                inmueble = new Inmueble(
                                    reader.GetString("Direccion"),
                                    reader.GetString("Uso"),
                                    reader.GetString("Estado")
                                )
                                {
                                    IdInmueble = reader.GetInt32("IdInmueble"),
                                    IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                    TipoNombre = reader.GetString("TipoNombre"),
                                    Ambientes = reader.GetInt32("Ambientes"),
                                    Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud"))
                                        ? (decimal?)null
                                        : Convert.ToDecimal(reader.GetDouble("Latitud")),
                                    Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud"))
                                        ? (decimal?)null
                                        : Convert.ToDecimal(reader.GetDouble("Longitud")),
                                    Precio = reader.GetDecimal("Precio"),
                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    PropietarioApellido = reader.GetString("PropietarioApellido"),
                                    PropietarioNombre = reader.GetString("PropietarioNombre"),
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
            return inmueble;
        }

        // Agrega un nuevo inmueble
        public int Create(Inmueble entidad)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new MySqlCommand())
                    {
                        command.Connection = (MySqlConnection)connection;
                        command.CommandText =
                            @"INSERT INTO inmueble (Direccion, Uso, IdTipoInmueble, Ambientes, Latitud, Longitud, Precio, Estado, IdPropietario)
                        VALUES (@Direccion, @Uso, @IdTipoInmueble, @Ambientes, @Latitud, @Longitud, @Precio, @Estado, @IdPropietario)";

                        command.Parameters.AddWithValue("@Direccion", entidad.Direccion);
                        command.Parameters.AddWithValue("@Uso", entidad.Uso);
                        command.Parameters.AddWithValue("@IdTipoInmueble", entidad.IdTipoInmueble);
                        command.Parameters.AddWithValue("@Ambientes", entidad.Ambientes);
                        command.Parameters.AddWithValue(
                            "@Latitud",
                            entidad.Latitud.HasValue
                                ? (object)Convert.ToDecimal(entidad.Latitud)
                                : DBNull.Value
                        );
                        command.Parameters.AddWithValue(
                            "@Longitud",
                            entidad.Longitud.HasValue
                                ? (object)Convert.ToDecimal(entidad.Longitud)
                                : DBNull.Value
                        );
                        command.Parameters.AddWithValue(
                            "@Precio",
                            Convert.ToDecimal(entidad.Precio)
                        );
                        command.Parameters.AddWithValue("@Estado", entidad.Estado);
                        command.Parameters.AddWithValue("@IdPropietario", entidad.IdPropietario);

                        command.ExecuteNonQuery();

                        // Retorna el ID del inmueble recién insertado
                        return (int)command.LastInsertedId;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        // Actualiza un inmueble existente
        public int Edit(Inmueble entidad)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();

                    using (var command = new MySqlCommand())
                    {
                        command.Connection = (MySqlConnection)connection;
                        command.CommandText =
                            @"UPDATE inmueble
                        SET Direccion = @Direccion,
                            Uso = @Uso,
                            IdTipoInmueble = @IdTipoInmueble,
                            Ambientes = @Ambientes,
                            Latitud = @Latitud,
                            Longitud = @Longitud,
                            Precio = @Precio,
                            Estado = @Estado,
                            IdPropietario = @IdPropietario
                        WHERE IdInmueble = @IdInmueble";

                        command.Parameters.AddWithValue("@IdInmueble", entidad.IdInmueble);
                        command.Parameters.AddWithValue("@Direccion", entidad.Direccion);
                        command.Parameters.AddWithValue("@Uso", entidad.Uso);
                        command.Parameters.AddWithValue("@IdTipoInmueble", entidad.IdTipoInmueble);
                        command.Parameters.AddWithValue("@Ambientes", entidad.Ambientes);
                        command.Parameters.AddWithValue(
                            "@Latitud",
                            entidad.Latitud.HasValue ? (object)entidad.Latitud.Value : DBNull.Value
                        );
                        command.Parameters.AddWithValue(
                            "@Longitud",
                            entidad.Longitud.HasValue
                                ? (object)entidad.Longitud.Value
                                : DBNull.Value
                        );
                        command.Parameters.AddWithValue(
                            "@Precio",
                            Convert.ToDecimal(entidad.Precio)
                        );
                        command.Parameters.AddWithValue("@Estado", entidad.Estado);
                        command.Parameters.AddWithValue("@IdPropietario", entidad.IdPropietario);

                        int filasAfectadas = command.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            throw new InvalidOperationException(
                                $"No se actualizó ningún registro con ID {entidad.IdInmueble}"
                            );
                        }

                        return filasAfectadas;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        // Elimina un inmueble por su ID (lógica de "baja lógica")
        public int Delete(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "UPDATE inmueble SET Activo = 0 WHERE IdInmueble = @IdInmueble",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdInmueble", id);
                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        // Obtiene inmuebles por su propietario
        public IList<Inmueble> ObtenerPorPropietario(int idPropietario)
        {
            var inmuebles = new List<Inmueble>();
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT * FROM inmueble WHERE IdPropietario = @IdPropietario AND Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdPropietario", idPropietario);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                inmuebles.Add(
                                    new Inmueble(
                                        reader.GetString("Direccion"),
                                        reader.GetString("Uso"),
                                        reader.GetString("Estado")
                                    )
                                    {
                                        IdInmueble = reader.GetInt32("IdInmueble"),
                                        IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                        Ambientes = reader.GetInt32("Ambientes"),
                                        Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader.GetDouble("Latitud")),
                                        Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader.GetDouble("Longitud")),
                                        Precio = reader.GetDecimal("Precio"),
                                        IdPropietario = reader.GetInt32("IdPropietario"),
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
            return inmuebles;
        }

        // Verifica si un inmueble está disponible en un rango de fechas
        public bool EstaDisponibleEnFechas(int idInmueble, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"SELECT COUNT(*)
                            FROM contrato c
                            INNER JOIN inmueble i ON c.IdInmueble = i.IdInmueble
                            WHERE c.IdInmueble = @IdInmueble
                              AND i.Activo = 1
                              AND ((@FechaInicio BETWEEN FechaInicio AND FechaFin) OR (@FechaFin BETWEEN FechaInicio AND FechaFin) OR (FechaInicio BETWEEN @FechaInicio AND @FechaFin) OR (FechaFin BETWEEN @FechaInicio AND @FechaFin))
                              AND c.Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdInmueble", idInmueble);
                        command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                        command.Parameters.AddWithValue("@FechaFin", fechaFin);
                        var count = Convert.ToInt32(command.ExecuteScalar());
                        return count == 0;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        // Método para obtener tipos de inmuebles
        public IEnumerable<TipoInmueble> ObtenerTiposInmuebles()
        {
            var tiposInmuebles = new List<TipoInmueble>();

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT * FROM tipoinmueble WHERE Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tiposInmuebles.Add(
                                    new TipoInmueble
                                    {
                                        IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                        Nombre = reader.GetString("Nombre"),
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

            return tiposInmuebles;
        }

        public IList<Inmueble> ObtenerTodosActivos()
        {
            var inmuebles = new List<Inmueble>();

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            @"SELECT i.*, t.Nombre AS TipoNombre, p.Apellido AS PropietarioApellido, p.Nombre AS PropietarioNombre
                      FROM inmueble i
                      JOIN tipoinmueble t ON i.IdTipoInmueble = t.IdTipoInmueble
                      JOIN propietario p ON i.IdPropietario = p.IdPropietario
                      WHERE i.Activo = 1 AND p.Activo = 1;",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                inmuebles.Add(
                                    new Inmueble(
                                        reader.GetString("Direccion"),
                                        reader.GetString("Uso"),
                                        reader.GetString("Estado")
                                    )
                                    {
                                        IdInmueble = reader.GetInt32("IdInmueble"),
                                        IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                        TipoNombre = reader.GetString("TipoNombre"),
                                        Ambientes = reader.GetInt32("Ambientes"),
                                        Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader.GetDouble("Latitud")),
                                        Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader.GetDouble("Longitud")),
                                        Precio = reader.GetDecimal("Precio"),
                                        IdPropietario = reader.GetInt32("IdPropietario"),
                                        PropietarioApellido = reader.GetString(
                                            "PropietarioApellido"
                                        ),
                                        PropietarioNombre = reader.GetString("PropietarioNombre"),
                                    }
                                );
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }

            return inmuebles;
        }
    }
}
