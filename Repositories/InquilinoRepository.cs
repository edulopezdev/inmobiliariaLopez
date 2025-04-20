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

        public InquilinoRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Métodos heredados de IRepositorio<T>

        /// Lista de Inquilinos
        public IList<Inquilino> Index()
        {
            var Inquilinos = new List<Inquilino>();
            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT IdInquilino, DNI, Apellido, Nombre, Telefono, Email FROM Inquilino WHERE Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Inquilinos.Add(
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
                    Console.WriteLine($"Error al obtener Inquilinos: {ex.Message}");
                    throw;
                }
            }
            return Inquilinos;
        }

        /// Obtiene un Inquilino por su ID
        public Inquilino? Details(int id)
        {
            Inquilino? Inquilino = null;
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
                                Inquilino = new Inquilino
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
            return Inquilino;
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

        /// Elimina un Inquilino por su ID
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

        // Métodos específicos de IRepositorioInquilino

        /// Obtiene un Inquilino por su DNI
        public Inquilino? ObtenerPorDNI(string dni)
        {
            Inquilino? Inquilino = null;
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
                                Inquilino = new Inquilino
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
            return Inquilino;
        }
    }
}
