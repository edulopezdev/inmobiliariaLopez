using System;
using System.Data; // Para trabajar con conexiones a bases de datos genéricas
using Microsoft.Extensions.Configuration; // Para leer la configuración del archivo appsettings.json
using MySql.Data.MySqlClient; // Para conectarnos específicamente a MySQL

namespace InmobiliariaLopez.Data
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor que inicializa la cadena de conexión.
        /// </summary>
        /// <param name="configuration">Instancia de IConfiguration para acceder a la configuración.</param>
        public DatabaseConnection(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MySql");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "La cadena de conexión 'MySql' no está configurada en appsettings.json."
                );
            }

            _connectionString = connectionString;
        }

        /// <summary>
        /// Crea y devuelve una nueva conexión a la base de datos.
        /// </summary>
        /// <returns>Una instancia de MySqlConnection lista para usar.</returns>
        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        /// <summary>
        /// Crea y devuelve una nueva conexión a la base de datos de forma asíncrona.
        /// </summary>
        /// <returns>Una tarea que representa la operación asincrónica y devuelve una instancia de MySqlConnection.</returns>
        public async Task<MySqlConnection> CreateConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
