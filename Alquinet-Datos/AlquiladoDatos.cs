using Alquinet_Entidad;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Alquinet_Datos
{
    public class AlquiladoDatos
    {
        public int Registrar(Alquilado alquilado, out string mensaje)
        {
            int returnId = -1;
            mensaje = string.Empty;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();

                    // Inserción del nuevo alquiler y obtener el ID generado
                    string query = "INSERT INTO Alquilado (cod_usuario, cod_agente, cod_propiedad, fecha, comision) " +
                                   "VALUES (@cod_usuario, @cod_agente, @cod_propiedad, @fecha, @comision) " + // Agregada la cláusula VALUES
                                   "RETURNING cod";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@cod_usuario", alquilado.Cod_usuario);
                        cmd.Parameters.AddWithValue("@cod_agente", alquilado.Cod_agente);
                        cmd.Parameters.AddWithValue("@cod_propiedad", alquilado.Cod_propiedad);
                        cmd.Parameters.AddWithValue("@fecha", alquilado.Fecha);
                        cmd.Parameters.AddWithValue("@comision", alquilado.Comision);

                        returnId = (int)cmd.ExecuteScalar();
                    }

                    mensaje = "Alquiler creado exitosamente";
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Código de error SQL para violación de restricción única en PostgreSQL
            {
                mensaje = "Violación de restricción única: " + ex.Message;
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
            }

            return returnId;
        }


        public bool Editar(Alquilado alquilado, out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();

                    // Actualización del alquiler
                    string query = "UPDATE Alquilado SET cod_agente = @cod_agente, fecha = @fecha, comision = @comision WHERE cod = @Cod";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@cod_agente", alquilado.Cod_agente);
                        cmd.Parameters.AddWithValue("@fecha", alquilado.Fecha);
                        cmd.Parameters.AddWithValue("@comision", alquilado.Comision);
                        cmd.Parameters.AddWithValue("@Cod", alquilado.Cod);

                        cmd.ExecuteNonQuery();
                    }

                    mensaje = "Alquiler actualizado exitosamente";
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
                    string query = "DELETE FROM Alquilado WHERE cod = @cod";
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

        public Alquilado GetById(int id)
        {
            Alquilado alquilado = new Alquilado();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT cod_usuario, cod_agente, cod_propiedad, fecha, comision FROM Alquilado WHERE cod = @Id";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read(); // Mueve el lector a la primera fila
                            alquilado.Cod = id;
                            alquilado.Cod_agente = Convert.ToInt32(dr["cod_agente"]);
                            alquilado.Cod_usuario = Convert.ToInt32(dr["cod_usuario"]);
                            alquilado.Cod_propiedad = Convert.ToInt32(dr["cod_propiedad"]);
                            alquilado.Fecha = DateTime.Parse(dr["fecha"].ToString());
                            alquilado.Comision = Convert.ToDouble(dr["comision"]);
                        }
                    }
                }
            }
            catch (Exception)
            {
                alquilado = null;
            }
            return alquilado;
        }

        public List<Alquilado> Listar()
        {
            List<Alquilado> lista = new List<Alquilado>();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM Alquilado";  // Asegúrate de que el nombre de la tabla sea correcto
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Alquilado()
                            {
                                Cod = Convert.ToInt32(dr["cod"]),
                                Cod_agente = Convert.ToInt32(dr["cod_agente"]),
                                Cod_propiedad = Convert.ToInt32(dr["cod_propiedad"]),
                                Cod_usuario = Convert.ToInt32(dr["cod_usuario"]),
                                Comision = Convert.ToDouble(dr["comision"]),
                                ComisionText = dr["comision"].ToString(),
                                Fecha = Convert.ToDateTime(dr["fecha"].ToString()),
                                FechaText = DateTime.Parse(dr["fecha"].ToString()).ToString("dd/MM/yyyy")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error message for further inspection
                Console.WriteLine($"Error: {ex.Message}");
                lista = new List<Alquilado>();
            }

            return lista;
        }

    }
}
