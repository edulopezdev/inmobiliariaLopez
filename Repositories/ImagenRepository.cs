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

                    lastInsertId = (long)(ulong)command.ExecuteScalar(); // Línea corregida
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
                        "SELECT IdImagen, Ruta, TipoImagen, IdRelacionado, Activo FROM imagen WHERE IdRelacionado = @IdRelacionado AND TipoImagen LIKE 'Inmueble%'";
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

        // Implementación de los métodos de IRepositorio<Imagen> (si es necesario para otras partes de tu aplicación)
        public IList<Imagen> Index()
        {
            // Implementación para obtener todas las imágenes (si lo necesitas)
            throw new NotImplementedException();
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
