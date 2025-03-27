using System.Data; //Esto es para usar conexiones genéricas a bases de datos
using MySql.Data.MySqlClient; //Esto es para trabajar específicamente con MySQL
using Microsoft.Extensions.Configuration; //Esto para leer la configuración del archivo appsettings.json

namespace InmobiliariaLopez.Data //Esto lo que hace es que la clase DatabaseConnection se encuentre en la carpeta Data
{
    public class DatabaseConnection  //Aca esta la clase que se encarga de crear la conexion
    {
        private readonly string _connectionString; //Aca vamos a guardar la cadena de conexion

        public DatabaseConnection(IConfiguration configuration) //Este es el constructor
        {
            //Aca vamos a obtener la cadena de conexion
            var connectionString = configuration.GetConnectionString("MySql");

            //Comprobamos que la cadena de conexion no este vacia
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'MySql' no está configurada en appsettings.json.");
            }

            //aca asignamos la cadena de conexion a la variable privada
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() //Este es el metodo que se encarga de crear la conexion
        {
            return new MySqlConnection(_connectionString); //Por ultimo creamos y devolvemos la conexion
        }
    }
}