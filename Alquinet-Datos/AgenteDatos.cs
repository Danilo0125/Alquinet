using Alquinet_Entidad;
using System;
using System.Collections.Generic;
using Npgsql;
using System.Data;

namespace Alquinet_Datos
{
    public class AgenteDatos
    {
        public int Registrar(Agente agente, out string mensaje)
        {
            int userId = -1;
            mensaje = string.Empty;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();

                    // Inserción del nuevo usuario y obtener el ID generado
                    string query = "INSERT INTO Agente (nombre, apellido, telefono, ci, correo, contraseña) " +
                                   "VALUES (@Nombre, @Apellido, @Telefono, @Ci, @Correo, @Contraseña) " +
                                   "RETURNING cod";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", agente.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", agente.Apellido);
                        cmd.Parameters.AddWithValue("@Telefono", agente.Telefono);
                        cmd.Parameters.AddWithValue("@Ci", agente.Ci);
                        cmd.Parameters.AddWithValue("@Correo", agente.Correo);
                        cmd.Parameters.AddWithValue("@Contraseña", agente.Contraseña);

                        userId = (int)cmd.ExecuteScalar();
                    }

                    mensaje = "Agente creado exitosamente";
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Código de error para violación de restricción única
            {
                mensaje = "El correo o el CI del agente ya existen";
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
            }

            return userId;
        }

        public bool Editar(Agente agente, out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();

                    // Actualización del usuario
                    string query = "UPDATE Agente SET nombre = @Nombre, apellido = @Apellido, telefono = @Telefono, " +
                                   "ci = @Ci, correo = @Correo, contraseña = @Contraseña WHERE cod = @Cod";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", agente.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", agente.Apellido);
                        cmd.Parameters.AddWithValue("@Telefono", agente.Telefono);
                        cmd.Parameters.AddWithValue("@Ci", agente.Ci);
                        cmd.Parameters.AddWithValue("@Correo", agente.Correo);
                        cmd.Parameters.AddWithValue("@Contraseña", agente.Contraseña);
                        cmd.Parameters.AddWithValue("@Cod", agente.Cod);

                        cmd.ExecuteNonQuery();
                    }

                    mensaje = "Agente actualizado exitosamente";
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Código de error para violación de restricción única
            {
                mensaje = "El correo o el CI del agente ya existen";
                return false;
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
                    string query = "DELETE FROM Agente WHERE cod = @cod"; // Sin LIMIT
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
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

        public Agente GetById(int id)
        {
            Agente agente = new Agente();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT nombre, apellido, telefono, correo, contraseña, ci FROM Agente WHERE cod = @Id";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read(); // Mueve el lector a la primera fila
                                agente.Cod = id;
                                agente.Nombre = rdr["nombre"].ToString();
                                agente.Apellido = rdr["apellido"].ToString();
                                agente.Telefono = rdr["telefono"].ToString();
                                agente.Correo = rdr["correo"].ToString();
                                agente.Contraseña = rdr["contraseña"].ToString();
                                agente.Ci = rdr["ci"].ToString();
                            }
                            else
                            {
                                agente.Nombre = "No se encontró al Usuario";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                agente.Nombre = "No se encontró al Usuario";
            }
            return agente;
        }

        public List<Agente> Listar()
        {
            List<Agente> lista = new List<Agente>();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM Agente";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        con.Open();
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Agente()
                                {
                                    Cod = Convert.ToInt32(dr["cod"]),
                                    Nombre = dr["nombre"].ToString(),
                                    Apellido = dr["apellido"].ToString(),
                                    Telefono = dr["telefono"].ToString(),
                                    Ci = dr["ci"].ToString(),
                                    Correo = dr["correo"].ToString(),
                                    Contraseña = dr["contraseña"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Agente>();
            }

            return lista;
        }

        public Agente GetByCorreo(string correo)
        {
            Agente agente = new Agente();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT cod, nombre, ci, apellido, telefono, correo, contraseña FROM Agente WHERE correo = @correo";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);
                        con.Open();
                        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read(); // Mueve el lector a la primera fila
                                agente.Cod = Convert.ToInt32(rdr["cod"]);
                                agente.Nombre = rdr["nombre"].ToString();
                                agente.Apellido = rdr["apellido"].ToString();
                                agente.Telefono = rdr["telefono"].ToString();
                                agente.Correo = rdr["correo"].ToString();
                                agente.Contraseña = rdr["contraseña"].ToString();
                                agente.Ci = rdr["ci"].ToString();
                            }
                            else
                            {
                                agente = null;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                agente = null;
            }
            return agente;
        }
    }
}
