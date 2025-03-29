using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Data;

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

    /// <summary>
    /// Obtiene todos los Inquilinos.
    /// </summary>
    /// <returns>Lista de Inquilinos.</returns>
    public IList<Inquilino> Index()
    {
      var Inquilinos = new List<Inquilino>();
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand("SELECT * FROM Inquilino", (MySqlConnection)connection))
          {
            using (var reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                Inquilinos.Add(new Inquilino
                {
                  IdInquilino = reader.GetInt32("IdInquilino"),
                  DNI = reader.GetString("DNI"),
                  Apellido = reader.GetString("Apellido"),
                  Nombre = reader.GetString("Nombre"),
                  Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? string.Empty : reader.GetString("Telefono"),
                  Email = reader.GetString("Email")
                });
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

    /// <summary>
    /// Obtiene un Inquilino por su ID.
    /// </summary>
    /// <param name="id">ID del Inquilino.</param>
    /// <returns>Inquilino encontrado o null si no existe.</returns>
    public Inquilino? Details(int id)
    {
      Inquilino? Inquilino = null;
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand("SELECT * FROM Inquilino WHERE IdInquilino = @IdInquilino", (MySqlConnection)connection))
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
                  Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? string.Empty : reader.GetString("Telefono"),
                  Email = reader.GetString("Email")
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

    /// <summary>
    /// Agrega un nuevo Inquilino.
    /// </summary>
    /// <param name="entidad">Datos del Inquilino a agregar.</param>
    /// <returns>ID del nuevo Inquilino.</returns>
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
            command.CommandText = @"
                            INSERT INTO Inquilino (DNI, Apellido, Nombre, Telefono, Email)
                            VALUES (@DNI, @Apellido, @Nombre, @Telefono, @Email)";
            command.Parameters.AddWithValue("@DNI", entidad.DNI);
            command.Parameters.AddWithValue("@Apellido", entidad.Apellido);
            command.Parameters.AddWithValue("@Nombre", entidad.Nombre);
            command.Parameters.AddWithValue("@Telefono", entidad.Telefono ?? (object)DBNull.Value);
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

    /// <summary>
    /// Actualiza un Inquilino existente.
    /// </summary>
    /// <param name="entidad">Datos actualizados del Inquilino.</param>
    /// <returns>Número de filas afectadas.</returns>
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
            command.CommandText = @"
                            UPDATE Inquilino
                            SET DNI = @DNI, Apellido = @Apellido, Nombre = @Nombre, Telefono = @Telefono, Email = @Email
                            WHERE IdInquilino = @IdInquilino";
            command.Parameters.AddWithValue("@IdInquilino", entidad.IdInquilino);
            command.Parameters.AddWithValue("@DNI", entidad.DNI);
            command.Parameters.AddWithValue("@Apellido", entidad.Apellido);
            command.Parameters.AddWithValue("@Nombre", entidad.Nombre);
            command.Parameters.AddWithValue("@Telefono", entidad.Telefono ?? (object)DBNull.Value);
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

    /// <summary>
    /// Elimina un Inquilino por su ID.
    /// </summary>
    /// <param name="id">ID del Inquilino a eliminar.</param>
    /// <returns>Número de filas afectadas.</returns>
    public int Delete(int id)
    {
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand("DELETE FROM Inquilino WHERE IdInquilino = @IdInquilino", (MySqlConnection)connection))
          {
            command.Parameters.AddWithValue("@IdInquilino", id);
            return command.ExecuteNonQuery(); // Retorna el número de filas afectadas
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al eliminar Inquilino: {ex.Message}");
          throw;
        }
      }
    }


    // Método específico de IRepositorioInquilino

    /// <summary>
    /// Obtiene un Inquilino por su DNI.
    /// </summary>
    /// <param name="dni">DNI del Inquilino.</param>
    /// <returns>Inquilino encontrado o null si no existe.</returns>
    public Inquilino? ObtenerPorDNI(string dni)
    {
      Inquilino? Inquilino = null;
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand("SELECT * FROM Inquilino WHERE DNI = @DNI", (MySqlConnection)connection))
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
                  Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? string.Empty : reader.GetString("Telefono"),
                  Email = reader.GetString("Email")
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