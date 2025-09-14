using Alquinet_Entidad;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Alquinet_Datos
{
    public class VisitaDatos
    {
        public int Registrar(Visita visita, out string mensaje)
        {
            int returnId = -1;
            mensaje = string.Empty;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();

                    string query = "INSERT INTO Visita (fecha, hora, cod_usuario, cod_propiedad, cod_agente, estado) " +
                                   "VALUES (@fecha, @hora, @cod_usuario, @cod_propiedad, @cod_agente, @estado) " + // Agregada la cláusula VALUES
                                   "RETURNING cod";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@fecha", visita.Fecha);
                        cmd.Parameters.AddWithValue("@hora", visita.Hora);
                        cmd.Parameters.AddWithValue("@cod_usuario", visita.Cod_usuario);
                        cmd.Parameters.AddWithValue("@cod_propiedad", visita.Cod_propiedad);
                        cmd.Parameters.AddWithValue("@cod_agente", visita.Cod_agente);
                        cmd.Parameters.AddWithValue("@estado", visita.Estado);

                        returnId = (int)cmd.ExecuteScalar();
                    }

                    mensaje = "Visita registrada exitosamente";
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Código para violación de restricción única
            {
                mensaje = ex.Message;
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
            }

            return returnId;
        }


        public bool Editar(Visita visita, out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();

                    string query = "UPDATE Visita SET fecha = @fecha, hora = @hora, cod_usuario = @cod_usuario, " +
                                   "cod_propiedad = @cod_propiedad, cod_agente = @cod_agente, estado = @estado WHERE cod = @cod";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@fecha", visita.Fecha);
                        cmd.Parameters.AddWithValue("@hora", visita.Hora);
                        cmd.Parameters.AddWithValue("@cod_usuario", visita.Cod_usuario);
                        cmd.Parameters.AddWithValue("@cod_propiedad", visita.Cod_propiedad);
                        cmd.Parameters.AddWithValue("@cod_agente", visita.Cod_agente);
                        cmd.Parameters.AddWithValue("@estado", visita.Estado);
                        cmd.Parameters.AddWithValue("@cod", visita.Cod);

                        cmd.ExecuteNonQuery();
                    }

                    mensaje = "Visita actualizada exitosamente";
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
                    string query = "DELETE FROM Visita WHERE cod = @cod";
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

        public Visita GetById(int id)
        {
            Visita visita = new Visita();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT fecha, hora, cod_usuario, cod_propiedad, cod_agente, estado FROM Visita WHERE cod = @Id";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            visita.Cod = id;
                            visita.Fecha = DateTime.Parse(dr["fecha"].ToString());
                            visita.Hora = TimeSpan.Parse(dr["hora"].ToString());
                            visita.Cod_usuario = Convert.ToInt32(dr["cod_usuario"]);
                            visita.Cod_propiedad = Convert.ToInt32(dr["cod_propiedad"]);
                            visita.Cod_agente = Convert.ToInt32(dr["cod_agente"]);
                            visita.Estado = dr["estado"].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                visita = null;
            }
            return visita;
        }

        public List<Visita> Listar(int cod, string estado)
        {
            List<Visita> lista = new List<Visita>();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM Visita WHERE cod_agente = @cod AND estado = @estado";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@cod", cod);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    con.Open();
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Visita()
                            {
                                Cod = Convert.ToInt32(dr["cod"]),
                                Fecha = DateTime.Parse(dr["fecha"].ToString()),
                                Hora = TimeSpan.Parse(dr["hora"].ToString()),
                                Cod_usuario = Convert.ToInt32(dr["cod_usuario"]),
                                Cod_propiedad = Convert.ToInt32(dr["cod_propiedad"]),
                                Cod_agente = Convert.ToInt32(dr["cod_agente"]),
                                Estado = dr["estado"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                lista = new List<Visita>();
            }

            return lista;
        }
    }
}
