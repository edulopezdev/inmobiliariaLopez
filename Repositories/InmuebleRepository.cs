using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Data;

namespace InmobiliariaLopez.Repositories
{
  public class InmuebleRepository : IRepositorioInmueble
  {
    private readonly DatabaseConnection _dbConnection;

    public InmuebleRepository(DatabaseConnection dbConnection)
    {
      _dbConnection = dbConnection;
    }

    // Métodos heredados de IRepositorio<T>

    // Lista todos los Inmuebles
    public IList<Inmueble> Index()
    {
      var inmuebles = new List<Inmueble>();
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand(
            "SELECT i.*, t.Nombre AS TipoNombre, p.Apellido AS PropietarioApellido, p.Nombre AS PropietarioNombre " +
            "FROM inmueble i " +
            "JOIN tipoinmueble t ON i.IdTipoInmueble = t.IdTipoInmueble " +
            "JOIN propietario p ON i.IdPropietario = p.IdPropietario", 
            (MySqlConnection)connection))
          {
            using (var reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                inmuebles.Add(new Inmueble
                {
                  IdInmueble = reader.GetInt32("IdInmueble"),
                  Direccion = reader.GetString("Direccion"),
                  Uso = reader.GetString("Uso"),
                  IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                  TipoNombre = reader.GetString("TipoNombre"),
                  Ambientes = reader.GetInt32("Ambientes"),
                  Coordenadas = reader.IsDBNull(reader.GetOrdinal("Coordenadas")) ? null : reader.GetString("Coordenadas"),
                  Precio = reader.GetDecimal("Precio"),
                  Estado = reader.GetString("Estado"),
                  IdPropietario = reader.GetInt32("IdPropietario"),
                  PropietarioApellido = reader.GetString("PropietarioApellido"),
                  PropietarioNombre = reader.GetString("PropietarioNombre")
                });
              }
            }
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al obtener inmuebles: {ex.Message}");
          throw;
        }
      }
      return inmuebles;
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
          using (var command = new MySqlCommand(
            "SELECT i.*, t.Nombre AS TipoNombre, p.Apellido AS PropietarioApellido, p.Nombre AS PropietarioNombre " +
            "FROM inmueble i " +
            "JOIN tipoinmueble t ON i.IdTipoInmueble = t.IdTipoInmueble " +
            "JOIN propietario p ON i.IdPropietario = p.IdPropietario " +
            "WHERE i.IdInmueble = @IdInmueble", 
            (MySqlConnection)connection))
          {
            command.Parameters.AddWithValue("@IdInmueble", id);
            using (var reader = command.ExecuteReader())
            {
              if (reader.Read())
              {
                inmueble = new Inmueble
                {
                  IdInmueble = reader.GetInt32("IdInmueble"),
                  Direccion = reader.GetString("Direccion"),
                  Uso = reader.GetString("Uso"),
                  IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                  TipoNombre = reader.GetString("TipoNombre"),
                  Ambientes = reader.GetInt32("Ambientes"),
                  Coordenadas = reader.IsDBNull(reader.GetOrdinal("Coordenadas")) ? null : reader.GetString("Coordenadas"),
                  Precio = reader.GetDecimal("Precio"),
                  Estado = reader.GetString("Estado"),
                  IdPropietario = reader.GetInt32("IdPropietario"),
                  PropietarioApellido = reader.GetString("PropietarioApellido"),
                  PropietarioNombre = reader.GetString("PropietarioNombre")
                };
              }
            }
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al obtener inmueble por ID: {ex.Message}");
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
            command.CommandText = @"
                            INSERT INTO inmueble (Direccion, Uso, IdTipoInmueble, Ambientes, Coordenadas, Precio, Estado, IdPropietario)
                            VALUES (@Direccion, @Uso, @IdTipoInmueble, @Ambientes, @Coordenadas, @Precio, @Estado, @IdPropietario)";
            command.Parameters.AddWithValue("@Direccion", entidad.Direccion);
            command.Parameters.AddWithValue("@Uso", entidad.Uso);
            command.Parameters.AddWithValue("@IdTipoInmueble", entidad.IdTipoInmueble);
            command.Parameters.AddWithValue("@Ambientes", entidad.Ambientes);
            command.Parameters.AddWithValue("@Coordenadas", entidad.Coordenadas ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Precio", entidad.Precio);
            command.Parameters.AddWithValue("@Estado", entidad.Estado);
            command.Parameters.AddWithValue("@IdPropietario", entidad.IdPropietario);
            command.ExecuteNonQuery();
            return (int)command.LastInsertedId; // Retorna el ID del nuevo inmueble
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al agregar inmueble: {ex.Message}");
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
            command.CommandText = @"
                            UPDATE inmueble
                            SET Direccion = @Direccion, Uso = @Uso, IdTipoInmueble = @IdTipoInmueble, Ambientes = @Ambientes,
                                Coordenadas = @Coordenadas, Precio = @Precio, Estado = @Estado, IdPropietario = @IdPropietario
                            WHERE IdInmueble = @IdInmueble";
            command.Parameters.AddWithValue("@IdInmueble", entidad.IdInmueble);
            command.Parameters.AddWithValue("@Direccion", entidad.Direccion);
            command.Parameters.AddWithValue("@Uso", entidad.Uso);
            command.Parameters.AddWithValue("@IdTipoInmueble", entidad.IdTipoInmueble);
            command.Parameters.AddWithValue("@Ambientes", entidad.Ambientes);
            command.Parameters.AddWithValue("@Coordenadas", entidad.Coordenadas ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Precio", entidad.Precio);
            command.Parameters.AddWithValue("@Estado", entidad.Estado);
            command.Parameters.AddWithValue("@IdPropietario", entidad.IdPropietario);
            return command.ExecuteNonQuery(); // Retorna el número de filas afectadas
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al actualizar inmueble: {ex.Message}");
          throw;
        }
      }
    }

    // Elimina un inmueble por su ID
    public int Delete(int id)
    {
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand("DELETE FROM inmueble WHERE IdInmueble = @IdInmueble", (MySqlConnection)connection))
          {
            command.Parameters.AddWithValue("@IdInmueble", id);
            return command.ExecuteNonQuery(); // Retorna el número de filas afectadas
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al eliminar inmueble: {ex.Message}");
          throw;
        }
      }
    }

    // Métodos específicos de IRepositorioInmueble

    // Obtiene inmuebles por su propietario
    public IList<Inmueble> ObtenerPorPropietario(int idPropietario)
    {
      var inmuebles = new List<Inmueble>();
      using (var connection = _dbConnection.CreateConnection())
      {
        try
        {
          connection.Open();
          using (var command = new MySqlCommand("SELECT * FROM inmueble WHERE IdPropietario = @IdPropietario", (MySqlConnection)connection))
          {
            command.Parameters.AddWithValue("@IdPropietario", idPropietario);
            using (var reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                inmuebles.Add(new Inmueble
                {
                  IdInmueble = reader.GetInt32("IdInmueble"),
                  Direccion = reader.GetString("Direccion"),
                  Uso = reader.GetString("Uso"),
                  IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                  Ambientes = reader.GetInt32("Ambientes"),
                  Coordenadas = reader.IsDBNull(reader.GetOrdinal("Coordenadas")) ? null : reader.GetString("Coordenadas"),
                  Precio = reader.GetDecimal("Precio"),
                  Estado = reader.GetString("Estado"),
                  IdPropietario = reader.GetInt32("IdPropietario")
                });
              }
            }
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al obtener inmuebles por propietario: {ex.Message}");
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
          using (var command = new MySqlCommand(
              "SELECT COUNT(*) FROM contrato WHERE IdInmueble = @IdInmueble AND " +
              "((@FechaInicio BETWEEN FechaInicio AND FechaFin) OR (@FechaFin BETWEEN FechaInicio AND FechaFin))",
              (MySqlConnection)connection))
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
          Console.WriteLine($"Error al verificar disponibilidad de inmueble: {ex.Message}");
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
          using (var command = new MySqlCommand("SELECT * FROM tipoinmueble", (MySqlConnection)connection))
          {
            using (var reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                tiposInmuebles.Add(new TipoInmueble
                {
                  IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                  Nombre = reader.GetString("Nombre")
                });
              }
            }
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error al obtener tipos de inmuebles: {ex.Message}");
          throw;
        }
      }

      return tiposInmuebles;
    }
  }
}
