using System.Data;
using InmobiliariaLopez.Data;
using InmobiliariaLopez.Models;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace InmobiliariaLopez.Repositories
{
    public class UsuarioRepository : IRepositorioUsuario
    {
        private readonly DatabaseConnection _dbConnection;
        private readonly ILogger<UsuarioRepository> _logger;

        public UsuarioRepository(DatabaseConnection dbConnection, ILogger<UsuarioRepository> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            Usuario? usuario = null;

            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @"SELECT * FROM Usuario WHERE Email = @email AND Activo = 1 LIMIT 1;";
                cmd.Parameters.AddWithValue("@email", email);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = reader.GetInt32("IdUsuario"),
                            Email = reader.GetString("Email"),
                            ContrasenaHasheada = reader.GetString("ContrasenaHasheada"),
                            Rol = reader.GetString("Rol"),
                            Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar"))
                                ? null
                                : reader.GetString("Avatar"),
                            FechaCreacion = reader.GetDateTime("FechaCreacion"),
                            Activo = reader.GetBoolean("Activo"),
                        };
                    }
                }
            }

            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            return usuario;
        }

        // Métodos obligatorios del IRepositorio - aún sin lógica (podés completarlos luego)

        public IList<Usuario> Index()
        {
            var usuarios = new List<Usuario>();
            using (var connection = _dbConnection.CreateConnection()) // Usamos la conexión que ya tienes definida
            {
                try
                {
                    connection.Open();
                    using (
                        var command = new MySqlCommand(
                            "SELECT IdUsuario, Email, ContrasenaHasheada, Rol, Avatar, FechaCreacion, Activo FROM usuario WHERE Activo = 1",
                            (MySqlConnection)connection
                        )
                    )
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usuarios.Add(
                                    new Usuario
                                    {
                                        IdUsuario = reader.GetInt32("IdUsuario"),
                                        Email = reader.GetString("Email"),
                                        ContrasenaHasheada = reader.GetString("ContrasenaHasheada"),
                                        Rol = reader.GetString("Rol"),
                                        Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar"))
                                            ? null
                                            : reader.GetString("Avatar"),
                                        FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                        Activo = reader.GetBoolean("Activo"),
                                    }
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
                    throw;
                }
            }
            return usuarios;
        }

        public Usuario? Details(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        @"SELECT IdUsuario, Email, ContrasenaHasheada, Rol, Avatar, FechaCreacion, Activo
                  FROM usuario
                  WHERE IdUsuario = @id",
                        (MySqlConnection)connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Email = reader["Email"].ToString()!,
                                ContrasenaHasheada = reader["ContrasenaHasheada"].ToString()!,
                                Rol = reader["Rol"].ToString()!,
                                Avatar =
                                    reader["Avatar"] != DBNull.Value
                                        ? reader["Avatar"].ToString()
                                        : null,
                                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                                Activo = Convert.ToBoolean(reader["Activo"]),
                            };
                        }
                    }
                }
            }

            return null; // Si no encuentra el usuario
        }

        public int Create(Usuario entidad)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        @"INSERT INTO usuario 
                            (Email, ContrasenaHasheada, Rol, Avatar, FechaCreacion, Activo)
                            VALUES 
                            (@Email, @ContrasenaHasheada, @Rol, @Avatar, @FechaCreacion, @Activo)",
                        (MySqlConnection)connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@Email", entidad.Email);
                    command.Parameters.AddWithValue(
                        "@ContrasenaHasheada",
                        entidad.ContrasenaHasheada
                    );
                    command.Parameters.AddWithValue("@Rol", entidad.Rol);
                    command.Parameters.AddWithValue(
                        "@Avatar",
                        (object?)entidad.Avatar ?? DBNull.Value
                    );
                    command.Parameters.AddWithValue("@FechaCreacion", entidad.FechaCreacion);
                    command.Parameters.AddWithValue("@Activo", entidad.Activo);

                    command.ExecuteNonQuery();

                    return (int)command.LastInsertedId;
                }
            }
        }

        public int Edit(Usuario entidad)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();

                // Solo eliminar el avatar si ha cambiado o si se proporciona un nuevo archivo
                if (!string.IsNullOrEmpty(entidad.Avatar))
                {
                    var usuarioExistente = Details(entidad.IdUsuario);
                    if (usuarioExistente != null && !string.IsNullOrEmpty(usuarioExistente.Avatar))
                    {
                        // Eliminar el avatar antiguo solo si hay uno nuevo o diferente
                        if (usuarioExistente.Avatar != entidad.Avatar)
                        {
                            var avatarAntiguoRuta = Path.Combine(
                                Directory.GetCurrentDirectory(),
                                "wwwroot",
                                usuarioExistente.Avatar.TrimStart('/')
                            );

                            if (File.Exists(avatarAntiguoRuta))
                            {
                                try
                                {
                                    File.Delete(avatarAntiguoRuta); // Elimina el archivo físicamente
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(
                                        $"Error al eliminar el avatar antiguo: {ex.Message}"
                                    );
                                }
                            }
                        }
                    }
                }

                // Ahora pocedemos a la actualización
                using (
                    var command = new MySqlCommand(
                        @"UPDATE usuario 
                  SET Email = @Email, 
                      ContrasenaHasheada = @ContrasenaHasheada, 
                      Rol = @Rol, 
                      Avatar = @Avatar, 
                      FechaCreacion = @FechaCreacion
                  WHERE IdUsuario = @IdUsuario",
                        (MySqlConnection)connection
                    )
                )
                {
                    // Parámetros para actualizar el usuario
                    command.Parameters.AddWithValue("@IdUsuario", entidad.IdUsuario);
                    command.Parameters.AddWithValue("@Email", entidad.Email);
                    command.Parameters.AddWithValue(
                        "@ContrasenaHasheada",
                        entidad.ContrasenaHasheada
                    );
                    command.Parameters.AddWithValue("@Rol", entidad.Rol);
                    command.Parameters.AddWithValue(
                        "@Avatar",
                        (object?)entidad.Avatar ?? DBNull.Value
                    );
                    command.Parameters.AddWithValue("@FechaCreacion", entidad.FechaCreacion);

                    // Ejecutar la actualización y devolver el número de filas afectadas
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int Delete(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (
                    var command = new MySqlCommand(
                        @"UPDATE usuario 
                  SET Activo = 0
                  WHERE IdUsuario = @IdUsuario",
                        (MySqlConnection)connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@IdUsuario", id);

                    // Ejecuta la actualización y devuelve el número de filas afectadas
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
