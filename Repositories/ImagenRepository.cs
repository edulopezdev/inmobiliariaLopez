using System;
using System.Collections.Generic;
using InmobiliariaLopez.Data;
using InmobiliariaLopez.Models;
using MySql.Data.MySqlClient;

namespace InmobiliariaLopez.Repositories
{
    public class ImagenRepository : IRepositorioImagen
    {
        private readonly DatabaseConnection _dbConnection;
        private readonly int _registrosPorPagina = 10; // Define la cantidad de registros por página

        public ImagenRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public int Create(Imagen imagen)
        {
            long lastInsertId = 0;
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        @"INSERT INTO imagen (Ruta, TipoImagen, IdRelacionado, Activo)
                    VALUES (@Ruta, @TipoImagen, @IdRelacionado, @Activo);
                    SELECT LAST_INSERT_ID();";
                    command.Parameters.AddWithValue("@Ruta", imagen.Ruta);
                    command.Parameters.AddWithValue("@TipoImagen", imagen.TipoImagen);
                    command.Parameters.AddWithValue("@IdRelacionado", imagen.IdRelacionado);
                    command.Parameters.AddWithValue("@Activo", imagen.Activo);

                    lastInsertId = (long)(ulong)command.ExecuteScalar();
                }
            }
            return (int)lastInsertId;
        }

        public int Delete(int id)
        {
            int affectedRows = 0;
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM imagen WHERE IdImagen = @IdImagen";
                    command.Parameters.AddWithValue("@IdImagen", id);
                    affectedRows = command.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

        public int Reemplazar(Imagen imagen)
        {
            int affectedRows = 0;
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        @"
                    UPDATE imagen
                    SET Ruta = @Ruta,
                        TipoImagen = @TipoImagen,
                        IdRelacionado = @IdRelacionado,
                        Activo = @Activo
                    WHERE IdImagen = @IdImagen;";
                    command.Parameters.AddWithValue("@IdImagen", imagen.IdImagen);
                    command.Parameters.AddWithValue("@Ruta", imagen.Ruta);
                    command.Parameters.AddWithValue("@TipoImagen", imagen.TipoImagen);
                    command.Parameters.AddWithValue("@IdRelacionado", imagen.IdRelacionado);
                    command.Parameters.AddWithValue("@Activo", imagen.Activo);

                    affectedRows = command.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

        public Imagen? ObtenerPorId(int id)
        {
            Imagen? imagen = null;
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        "SELECT IdImagen, Ruta, TipoImagen, IdRelacionado, Activo FROM imagen WHERE IdImagen = @IdImagen";
                    command.Parameters.AddWithValue("@IdImagen", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imagen = new Imagen
                            {
                                IdImagen = reader.GetInt32("IdImagen"),
                                Ruta = reader.GetString("Ruta"),
                                TipoImagen = reader.GetString("TipoImagen"),
                                IdRelacionado = reader.GetInt32("IdRelacionado"),
                                Activo = reader.GetBoolean("Activo"),
                            };
                        }
                    }
                }
            }
            return imagen;
        }

        public IList<Imagen> ObtenerPorInmueble(int idInmueble)
        {
            var imagenes = new List<Imagen>();
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        "SELECT IdImagen, Ruta, TipoImagen, IdRelacionado, Activo FROM imagen WHERE IdRelacionado = @IdRelacionado AND TipoImagen LIKE 'Inmueble%' AND Activo = 1";
                    command.Parameters.AddWithValue("@IdRelacionado", idInmueble);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            imagenes.Add(
                                new Imagen
                                {
                                    IdImagen = reader.GetInt32("IdImagen"),
                                    Ruta = reader.GetString("Ruta"),
                                    TipoImagen = reader.GetString("TipoImagen"),
                                    IdRelacionado = reader.GetInt32("IdRelacionado"),
                                    Activo = reader.GetBoolean("Activo"),
                                }
                            );
                        }
                    }
                }
            }
            return imagenes;
        }

        // Implementación de los métodos de IRepositorio<Imagen> con paginación
        public IList<Imagen> Index(int pagina = 1)
        {
            var imagenes = new List<Imagen>();
            int skipAmount = (pagina - 1) * _registrosPorPagina;

            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        @"
                        SELECT IdImagen, Ruta, TipoImagen, IdRelacionado, Activo
                        FROM imagen
                        WHERE Activo = 1
                        LIMIT @limit OFFSET @offset;";
                    command.Parameters.AddWithValue("@limit", _registrosPorPagina);
                    command.Parameters.AddWithValue("@offset", skipAmount);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            imagenes.Add(
                                new Imagen
                                {
                                    IdImagen = reader.GetInt32("IdImagen"),
                                    Ruta = reader.GetString("Ruta"),
                                    TipoImagen = reader.GetString("TipoImagen"),
                                    IdRelacionado = reader.GetInt32("IdRelacionado"),
                                    Activo = reader.GetBoolean("Activo"),
                                }
                            );
                        }
                    }
                }
            }
            return imagenes;
        }

        public int ObtenerTotal()
        {
            int totalRegistros = 0;
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        "SELECT COUNT(*) FROM imagen WHERE Activo = 1",
                        (MySqlConnection)connection
                    )
                )
                {
                    totalRegistros = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return totalRegistros;
        }

        public Imagen? Details(int id)
        {
            // Reutilizamos ObtenerPorId
            return ObtenerPorId(id);
        }

        public int Edit(Imagen entidad)
        {
            // Reutilizamos Reemplazar
            return Reemplazar(entidad);
        }
    }
}
