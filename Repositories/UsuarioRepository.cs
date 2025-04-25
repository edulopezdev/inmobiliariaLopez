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
            throw new NotImplementedException();
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

                    System.Diagnostics.Debug.WriteLine(">>>>> INSERT ejecutado");
                    return (int)command.LastInsertedId;
                }
            }
        }

        public int Edit(Usuario entidad)
        {
            throw new NotImplementedException();
        }

        public int Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
