using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Data;

namespace InmobiliariaLopez.Repositories
{
  public class PropietarioRepository : IRepositorioPropietario
  {
    private readonly DatabaseConnection _dbConnection;

    public PropietarioRepository(DatabaseConnection dbConnection)
    {
      _dbConnection = dbConnection;
    }

    // Métodos heredados de IRepositorio<T>

    /// <summary>
    /// Obtiene todos los propietarios.
    /// </summary>
    /// <returns>Lista de propietarios.</returns>
    public IList<Propietario> Index()
    {
      var propietarios = new List<Propietario>();
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand("SELECT * FROM propietario", (MySqlConnection)connection))
          {
            using (var reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                propietarios.Add(new Propietario
                {
                  IdPropietario = reader.GetInt32("IdPropietario"),
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
          Console.WriteLine($"Error al obtener propietarios: {ex.Message}");
          throw;
        }
      }
      return propietarios;
    }

    /// <summary>
    /// Obtiene un propietario por su ID.
    /// </summary>
    /// <param name="id">ID del propietario.</param>
    /// <returns>Propietario encontrado o null si no existe.</returns>
    public Propietario? Details(int id)
    {
      Propietario? propietario = null;
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand("SELECT * FROM propietario WHERE IdPropietario = @IdPropietario", (MySqlConnection)connection))
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
                  Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? string.Empty : reader.GetString("Telefono"),
                  Email = reader.GetString("Email")
                };
              }
            }
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al obtener propietario por ID: {ex.Message}");
          throw;
        }
      }
      return propietario;
    }

    /// <summary>
    /// Agrega un nuevo propietario.
    /// </summary>
    /// <param name="entidad">Datos del propietario a agregar.</param>
    /// <returns>ID del nuevo propietario.</returns>
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
            command.CommandText = @"
                            INSERT INTO propietario (DNI, Apellido, Nombre, Telefono, Email)
                            VALUES (@DNI, @Apellido, @Nombre, @Telefono, @Email)";
            command.Parameters.AddWithValue("@DNI", entidad.DNI);
            command.Parameters.AddWithValue("@Apellido", entidad.Apellido);
            command.Parameters.AddWithValue("@Nombre", entidad.Nombre);
            command.Parameters.AddWithValue("@Telefono", entidad.Telefono ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", entidad.Email);
            command.ExecuteNonQuery();
            return (int)command.LastInsertedId; // Retorna el ID del nuevo propietario
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al agregar propietario: {ex.Message}");
          throw;
        }
      }
    }

    /// <summary>
    /// Actualiza un propietario existente.
    /// </summary>
    /// <param name="entidad">Datos actualizados del propietario.</param>
    /// <returns>Número de filas afectadas.</returns>
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
            command.CommandText = @"
                            UPDATE propietario
                            SET DNI = @DNI, Apellido = @Apellido, Nombre = @Nombre, Telefono = @Telefono, Email = @Email
                            WHERE IdPropietario = @IdPropietario";
            command.Parameters.AddWithValue("@IdPropietario", entidad.IdPropietario);
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
          Console.WriteLine($"Error al actualizar propietario: {ex.Message}");
          throw;
        }
      }
    }

    /// <summary>
    /// Elimina un propietario por su ID.
    /// </summary>
    /// <param name="id">ID del propietario a eliminar.</param>
    /// <returns>Número de filas afectadas.</returns>
    public int Delete(int id)
    {
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand("DELETE FROM propietario WHERE IdPropietario = @IdPropietario", (MySqlConnection)connection))
          {
            command.Parameters.AddWithValue("@IdPropietario", id);
            return command.ExecuteNonQuery(); // Retorna el número de filas afectadas
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al eliminar propietario: {ex.Message}");
          throw;
        }
      }
    }


    // Método específico de IRepositorioPropietario

    /// <summary>
    /// Obtiene un propietario por su DNI.
    /// </summary>
    /// <param name="dni">DNI del propietario.</param>
    /// <returns>Propietario encontrado o null si no existe.</returns>
    public Propietario? ObtenerPorDNI(string dni)
    {
      Propietario? propietario = null;
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand("SELECT * FROM propietario WHERE DNI = @DNI", (MySqlConnection)connection))
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
                  Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? string.Empty : reader.GetString("Telefono"),
                  Email = reader.GetString("Email")
                };
              }
            }
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al obtener propietario por DNI: {ex.Message}");
          throw;
        }
      }
      return propietario;
    }
  }
}