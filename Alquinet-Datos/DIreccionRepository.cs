using Alquinet_Entidad;
using Alquinet_Entidad.ClasesPropiedad;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Policy;
using System.Threading;

namespace Alquinet_Datos
{
    public class DireccionRepository
    {
        public int Registrar(Direccion direccion, out string mensaje)
        {
            int returnId = -1;
            mensaje = string.Empty;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();

                    // Inserción de la nueva dirección y obtener el ID generado
                    string query = "INSERT INTO Direccion (cod_postal, calle, barrio, zona, longitud, latitud) " +
                                   "VALUES (@cod_postal, @calle, @barrio, @zona, @longitud, @latitud) " +
                                   "RETURNING cod";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        // Agregar parámetros a la consulta
                        cmd.Parameters.AddWithValue("@cod_postal", Convert.ToInt32(direccion.Cod_pos));
                        cmd.Parameters.AddWithValue("@calle", direccion.Calle);
                        cmd.Parameters.AddWithValue("@barrio", direccion.Barrio);
                        cmd.Parameters.AddWithValue("@zona", direccion.Zona);
                        cmd.Parameters.AddWithValue("@longitud", direccion.Longitud);
                        cmd.Parameters.AddWithValue("@latitud", direccion.Latitud);

                        // Ejecutar el comando y obtener el ID generado
                        returnId = (int)cmd.ExecuteScalar();
                    }

                    mensaje = "Dirección creada exitosamente";
                }
            }
            catch (PostgresException ex) // Captura de errores de PostgreSQL
            {
                mensaje = ex.Message;
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
            }

            return returnId;
        }


        public bool Editar(Direccion direccion, out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();

                    // Actualización de la dirección
                    string query = "UPDATE Direccion SET cod_postal = @cod_postal, calle = @calle, barrio = @barrio, " +
                                   "zona = @zona, longitud = @longitud, latitud = @latitud WHERE cod = @Cod";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@cod_postal", direccion.Cod_pos);
                        cmd.Parameters.AddWithValue("@calle", direccion.Calle);
                        cmd.Parameters.AddWithValue("@barrio", direccion.Barrio);
                        cmd.Parameters.AddWithValue("@zona", direccion.Zona);
                        cmd.Parameters.AddWithValue("@longitud", direccion.Longitud);
                        cmd.Parameters.AddWithValue("@latitud", direccion.Latitud);
                        cmd.Parameters.AddWithValue("@Cod", direccion.Cod);

                        cmd.ExecuteNonQuery();
                    }

                    mensaje = "Dirección actualizada exitosamente";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
                return false;
            }

            return true;
        }

        public bool Eliminar(int cod, out string mensaje)
        {
            mensaje = string.Empty;
            bool result = false;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "DELETE FROM Direccion WHERE cod = @cod";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@cod", cod);
                    con.Open();
                    result = cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                result = false;
                mensaje = ex.Message;
            }
            return result;
        }

        public Direccion GetById(int id)
        {
            Direccion direccion = new Direccion();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT cod_postal, calle, barrio, zona, longitud, latitud FROM Direccion WHERE cod = @Id";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read(); // Mueve el lector a la primera fila
                            direccion.Cod = id;
                            direccion.Cod_pos = Convert.ToString(dr["cod_postal"]);
                            direccion.Calle = Convert.ToString(dr["calle"]);
                            direccion.Barrio = Convert.ToString(dr["barrio"]);
                            direccion.Zona = Convert.ToString(dr["zona"]);
                            direccion.Longitud = Convert.ToDecimal(dr["longitud"]);
                            direccion.Latitud = Convert.ToDecimal(dr["latitud"]);
                        }
                    }
                }
            }
            catch (Exception)
            {
                direccion = null;
            }
            return direccion;
        }

        public List<Direccion> Listar()
        {
            List<Direccion> lista = new List<Direccion>();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM Direccion";  // Asegúrate de que el nombre de la tabla sea correcto
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Direccion()
                            {
                                Cod = Convert.ToInt32(dr["cod"]),
                                Cod_pos = Convert.ToString(dr["cod_postal"]),
                                Calle = Convert.ToString(dr["calle"]),
                                Barrio = Convert.ToString(dr["barrio"]),
                                Zona = Convert.ToString(dr["zona"]),
                                Longitud = Convert.ToDecimal(dr["longitud"]),
                                Latitud = Convert.ToDecimal(dr["latitud"]),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error message for further inspection
                Console.WriteLine($"Error: {ex.Message}");
                lista = new List<Direccion>();
            }

            return lista;
        }
    }
}
