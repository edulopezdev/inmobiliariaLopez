using System;
using System.Collections.Generic;
using InmobiliariaLopez.Data;
using InmobiliariaLopez.Models;
using MySql.Data.MySqlClient;

namespace InmobiliariaLopez.Repositories
{
    public class ReporteRepository : IRepositorioReporte
    {
        private readonly DatabaseConnection _dbConnection;

        public ReporteRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Método para obtener inmuebles con propietarios
        public IList<InmuebleConPropietarioViewModel> ObtenerInmueblesConPropietarios(
            bool soloDisponibles
        )
        {
            var inmuebles = new List<InmuebleConPropietarioViewModel>();

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();

                    // Consulta SQL para obtener los inmuebles con propietarios
                    var query =
                        @"
                        SELECT 
                            i.IdInmueble, 
                            i.Direccion, 
                            i.Uso, 
                            t.Nombre AS TipoInmueble, 
                            i.Ambientes, 
                            i.Precio, 
                            i.Estado,
                            p.Nombre AS PropietarioNombre, 
                            p.Apellido AS PropietarioApellido, 
                            p.Email AS PropietarioEmail, 
                            p.Telefono AS PropietarioTelefono
                        FROM 
                            inmueble i
                        JOIN 
                            tipoinmueble t ON i.IdTipoInmueble = t.IdTipoInmueble
                        JOIN 
                            propietario p ON i.IdPropietario = p.IdPropietario
                        WHERE 
                            (@soloDisponibles = 0 OR i.Estado = 'Disponible')";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue(
                            "@soloDisponibles",
                            soloDisponibles ? 1 : 0
                        );

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                inmuebles.Add(
                                    new InmuebleConPropietarioViewModel
                                    {
                                        IdInmueble = reader.GetInt32("IdInmueble"),
                                        Direccion = reader.GetString("Direccion"),
                                        Uso = reader.GetString("Uso"),
                                        TipoInmueble = reader.GetString("TipoInmueble"),
                                        Ambientes = reader.GetInt32("Ambientes"),
                                        Precio = reader.GetDecimal("Precio"),
                                        Estado = reader.GetString("Estado"),
                                        PropietarioNombre = reader.GetString("PropietarioNombre"),
                                        PropietarioApellido = reader.GetString(
                                            "PropietarioApellido"
                                        ),
                                        PropietarioEmail = reader.GetString("PropietarioEmail"),
                                        PropietarioTelefono = reader.GetString(
                                            "PropietarioTelefono"
                                        ),
                                    }
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Aquí puedes manejar el error de forma adecuada
                    throw new Exception("Error al obtener los inmuebles con propietarios", ex);
                }
            }

            return inmuebles;
        }

        public IList<ContratoViewModel> ObtenerContratosVigentes()
        {
            var contratos = new List<ContratoViewModel>();

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();

                    var query =
                        @"
                        SELECT 
                          c.IdContrato, 
                            c.FechaInicio, 
                            c.FechaFin, 
                          i.Direccion AS InmuebleDireccion, 
                             CONCAT(i.Direccion, ' - ', t.Nombre) AS InmuebleDescripcion, 
                               inq.Nombre AS InquilinoNombre
                          FROM 
                            contrato c
                              JOIN 
                            inmueble i ON c.IdInmueble = i.IdInmueble
                                JOIN 
                             tipoinmueble t ON i.IdTipoInmueble = t.IdTipoInmueble
                                JOIN 
                             inquilino inq ON c.IdInquilino = inq.IdInquilino
                                 WHERE 
                              c.FechaFin >= @FechaActual";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@FechaActual", DateTime.Now);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                contratos.Add(
                                    new ContratoViewModel
                                    {
                                        IdContrato = reader.GetInt32("IdContrato"),
                                        FechaInicio = reader.GetDateTime("FechaInicio"),
                                        FechaFin = reader.GetDateTime("FechaFin"),
                                        InquilinoNombre = reader.GetString("InquilinoNombre"),
                                        InmuebleDireccion = reader.GetString("InmuebleDireccion"),
                                    }
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener contratos vigentes", ex);
                }
            }

            return contratos;
        }

        public IList<InmuebleDesocupadoViewModel> ObtenerInmueblesDesocupados(
            DateTime desde,
            DateTime hasta
        )
        {
            var inmueblesDesocupados = new List<InmuebleDesocupadoViewModel>();

            using (var connection = _dbConnection.CreateConnection())
            {
                try
                {
                    connection.Open();

                    var query =
                        @"
    SELECT 
        i.IdInmueble, 
        i.Direccion, 
        t.Nombre AS TipoInmueble, 
        i.Precio, 
        c.FechaFin AS FechaDesocupado
    FROM 
        inmueble i
    LEFT JOIN 
        contrato c ON i.IdInmueble = c.IdInmueble
    JOIN 
        tipoinmueble t ON i.IdTipoInmueble = t.IdTipoInmueble
    WHERE 
        c.FechaFin BETWEEN @Desde AND @Hasta";

                    using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                    {
                        command.Parameters.AddWithValue("@Desde", desde);
                        command.Parameters.AddWithValue("@Hasta", hasta);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                inmueblesDesocupados.Add(
                                    new InmuebleDesocupadoViewModel
                                    {
                                        IdInmueble = reader.GetInt32("IdInmueble"),
                                        Direccion = reader.GetString("Direccion"),
                                        TipoInmueble = reader.GetString("TipoInmueble"),
                                        Precio = reader.GetDecimal("Precio"),
                                        FechaDesocupado = reader.GetDateTime("FechaDesocupado"),
                                    }
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener inmuebles desocupados", ex);
                }
            }

            return inmueblesDesocupados;
        }
    }
}
