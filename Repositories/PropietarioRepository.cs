using System;
using System.Collections.Generic;
using InmobiliariaLopez.Data;
using InmobiliariaLopez.Models;
using MySql.Data.MySqlClient;

namespace InmobiliariaLopez.Repositories
{
    public class PropietarioRepository : IRepositorioPropietario
    {
        private readonly DatabaseConnection _dbConnection;
        private readonly int _registrosPorPagina = 10;

        public PropietarioRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Propiedad pública para acceder a la cantidad de registros por página
        public int RegistrosPorPagina => _registrosPorPagina;

        // Métodos heredados de IRepositorio<T>

        // Método para obtener todos los propietarios
        public IList<Propietario> Index(int pagina = 1)
        {
            var propietarios = new List<Propietario>();
            int skipAmount = (pagina - 1) * _registrosPorPagina;

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new MySqlCommand())
                    {
                        command.Connection = (MySqlConnection)connection;
                        command.CommandText =
                            @"
                    SELECT IdPropietario, DNI, Apellido, Nombre, Telefono, Email
                    FROM propietario
                    WHERE Activo = 1
                    LIMIT @limit OFFSET @offset";
                        command.Parameters.AddWithValue("@limit", _registrosPorPagina);
                        command.Parameters.AddWithValue("@offset", skipAmount);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                propietarios.Add(
                                    new Propietario
                                    {
                                        IdPropietario = reader.GetInt32("IdPropietario"),
                                        DNI = reader.GetString("DNI"),
                                        Apellido = reader.GetString("Apellido"),
                                        Nombre = reader.GetString("Nombre"),
                                        Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono"))
                                            ? string.Empty
                                            : reader.GetString("Telefono"),
                                        Email = reader.GetString("Email"),
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
            return propietarios;
        }

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
                            "SELECT COUNT(*) FROM propietario WHERE Activo = 1",
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

        /// Obtiene un propietario por su ID.
        public Propietario? Details(int id)
        {
            Propietario? propietario = null;
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT IdPropietario, DNI, Apellido, Nombre, Telefono, Email FROM propietario WHERE IdPropietario = @IdPropietario AND Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdPropietario", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                propietario = new Propietario
                                {
                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    DNI = reader.GetString("DNI"),
                                    Apellido = reader.GetString("Apellido"),
                                    Nombre = reader.GetString("Nombre"),
                                    Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono"))
                                        ? string.Empty
                                        : reader.GetString("Telefono"),
                                    Email = reader.GetString("Email"),
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
            return propietario;
        }

        /// Agrega un nuevo propietario
        public int Create(Propietario entidad)
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
                            @"
                            INSERT INTO propietario (DNI, Apellido, Nombre, Telefono, Email)
                            VALUES (@DNI, @Apellido, @Nombre, @Telefono, @Email)";
                        command.Parameters.AddWithValue("@DNI", entidad.DNI);
                        command.Parameters.AddWithValue("@Apellido", entidad.Apellido);
                        command.Parameters.AddWithValue("@Nombre", entidad.Nombre);
                        command.Parameters.AddWithValue(
                            "@Telefono",
                            entidad.Telefono ?? (object)DBNull.Value
                        );
                        command.Parameters.AddWithValue("@Email", entidad.Email);
                        command.ExecuteNonQuery();
                        return (int)command.LastInsertedId; // Retorna el ID del nuevo propietario
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// Actualiza un propietario existente
        public int Edit(Propietario entidad)
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
                            @"
                            UPDATE propietario
                            SET DNI = @DNI, Apellido = @Apellido, Nombre = @Nombre, Telefono = @Telefono, Email = @Email
                            WHERE IdPropietario = @IdPropietario";
                        command.Parameters.AddWithValue("@IdPropietario", entidad.IdPropietario);
                        command.Parameters.AddWithValue("@DNI", entidad.DNI);
                        command.Parameters.AddWithValue("@Apellido", entidad.Apellido);
                        command.Parameters.AddWithValue("@Nombre", entidad.Nombre);
                        command.Parameters.AddWithValue(
                            "@Telefono",
                            entidad.Telefono ?? (object)DBNull.Value
                        );
                        command.Parameters.AddWithValue("@Email", entidad.Email);
                        return command.ExecuteNonQuery(); // Retorna el número de filas afectadas
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// Elimina un propietario por su ID
        public int Delete(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "UPDATE propietario SET Activo = 0 WHERE IdPropietario = @IdPropietario",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdPropietario", id);
                        return command.ExecuteNonQuery(); // Retorna el número de filas afectadas
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        // Método específico de IRepositorioPropietario

        /// Obtiene un propietario por su DNI
        public Propietario? ObtenerPorDNI(string dni)
        {
            Propietario? propietario = null;
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT IdPropietario, DNI, Apellido, Nombre, Telefono, Email FROM propietario WHERE DNI = @DNI AND Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@DNI", dni);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                propietario = new Propietario
                                {
                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    DNI = reader.GetString("DNI"),
                                    Apellido = reader.GetString("Apellido"),
                                    Nombre = reader.GetString("Nombre"),
                                    Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono"))
                                        ? string.Empty
                                        : reader.GetString("Telefono"),
                                    Email = reader.GetString("Email"),
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
            return propietario;
        }
    }
}
