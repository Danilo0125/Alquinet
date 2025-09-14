using System;
using Npgsql;

namespace Alquinet_Datos
{
    public class Conexion
    {
        public static string cn = "Host=localhost; Port=5432; Database=db_aquinet; User Id=postgres; Password=12345678;";

        public void ProbarConexion()
        {
            using (NpgsqlConnection con = new NpgsqlConnection(cn))
            {
                try
                {
                    con.Open();
                    Console.WriteLine("Conexión exitosa!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en la conexión: {ex.Message}");
                }
            }
        }
    }
}
