using System;
using System.Collections.Generic;
using InmobiliariaLopez.Data;
using InmobiliariaLopez.Models;
using MySql.Data.MySqlClient;

namespace InmobiliariaLopez.Repositories
{
    public class InquilinoRepository : IRepositorioInquilino
    {
        private readonly DatabaseConnection _dbConnection;
        private readonly int _registrosPorPagina = 10; // Define la cantidad de registros por página

        public InquilinoRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// Lista de Inquilinos con paginación
        public IList<Inquilino> Index(int pagina = 1)
        {
            var inquilinos = new List<Inquilino>();
            int skipAmount = (pagina - 1) * _registrosPorPagina;

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT IdInquilino, DNI, Apellido, Nombre, Telefono, Email FROM Inquilino WHERE Activo = 1 LIMIT @limit OFFSET @offset",
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
                                inquilinos.Add(
                                    new Inquilino
                                    {
                                        IdInquilino = reader.GetInt32("IdInquilino"),
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
                    Console.WriteLine($"Error al obtener Inquilinos paginados: {ex.Message}");
                    throw;
                }
            }
            return inquilinos;
        }

        /// Obtiene la cantidad total de inquilinos activos
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
                            "SELECT COUNT(*) FROM Inquilino WHERE Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        totalRegistros = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener el total de inquilinos: {ex.Message}");
                    throw;
                }
            }
            return totalRegistros;
        }

        /// Obtiene un Inquilino por su ID
        public Inquilino? Details(int id)
        {
            Inquilino? inquilino = null;
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT IdInquilino, DNI, Apellido, Nombre, Telefono, Email FROM Inquilino WHERE IdInquilino = @IdInquilino AND Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@IdInquilino", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                inquilino = new Inquilino
                                {
                                    IdInquilino = reader.GetInt32("IdInquilino"),
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
                    Console.WriteLine($"Error al obtener Inquilino por ID: {ex.Message}");
                    throw;
                }
            }
            return inquilino;
        }

        /// Agrega un nuevo Inquilino
        public int Create(Inquilino entidad)
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
                            INSERT INTO Inquilino (DNI, Apellido, Nombre, Telefono, Email)
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
                        return (int)command.LastInsertedId; // Retorna el ID del nuevo Inquilino
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al agregar Inquilino: {ex.Message}");
                    throw;
                }
            }
        }

        /// Actualiza un Inquilino existente
        public int Edit(Inquilino entidad)
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
                            UPDATE Inquilino
                            SET DNI = @DNI, Apellido = @Apellido, Nombre = @Nombre, Telefono = @Telefono, Email = @Email
                            WHERE IdInquilino = @IdInquilino";
                        command.Parameters.AddWithValue("@IdInquilino", entidad.IdInquilino);
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
                    Console.WriteLine($"Error al actualizar Inquilino: {ex.Message}");
                    throw;
                }
            }
        }

        /// Elimina un Inquilino por su ID (baja lógica)
        public int Delete(int id)
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
                            UPDATE Inquilino
                            SET Activo = 0
                            WHERE IdInquilino = @IdInquilino";
                        command.Parameters.AddWithValue("@IdInquilino", id);
                        return command.ExecuteNonQuery(); // Retorna el número de filas afectadas
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al dar de baja Inquilino: {ex.Message}");
                    throw;
                }
            }
        }

        /// Obtiene un Inquilino por su DNI
        public Inquilino? ObtenerPorDNI(string dni)
        {
            Inquilino? inquilino = null;
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT * FROM Inquilino WHERE DNI = @DNI AND Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@DNI", dni);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                inquilino = new Inquilino
                                {
                                    IdInquilino = reader.GetInt32("IdInquilino"),
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
                    Console.WriteLine($"Error al obtener Inquilino por DNI: {ex.Message}");
                    throw;
                }
            }
            return inquilino;
        }
    }
}
