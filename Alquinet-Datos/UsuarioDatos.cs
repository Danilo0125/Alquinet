using Alquinet_Entidad;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Alquinet_Datos
{
    public class UsuarioDatos
    {
        public int Registrar(Usuario usuario, out string mensaje)
        {
            int userId = -1;
            mensaje = string.Empty;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();

                    // Inserción del nuevo usuario y devolver el ID generado
                    string query = "INSERT INTO Usuario (nombre, apellido, telefono, ci, correo, contraseña) " +
                                   "VALUES (@Nombre, @Apellido, @Telefono, @Ci, @Correo, @Contraseña) " +
                                   "RETURNING cod;";  // Asegúrate de que 'cod' es el nombre correcto de la columna que almacena el ID

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("Nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("Apellido", usuario.Apellido);
                        cmd.Parameters.AddWithValue("Telefono", usuario.Telefono);
                        cmd.Parameters.AddWithValue("Ci", usuario.Ci);
                        cmd.Parameters.AddWithValue("Correo", usuario.Correo);
                        cmd.Parameters.AddWithValue("Contraseña", usuario.Contraseña);

                        userId = (int)cmd.ExecuteScalar(); // Ejecuta la consulta y obtiene el ID generado
                    }

                    mensaje = "Usuario creado exitosamente";
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Código de error PostgreSQL para violación de restricción única
            {
                mensaje = "El correo o el CI del usuario ya existen";
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
            }

            return userId;
        }



        public bool Editar(Usuario usuario, out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();

                    // Actualización del usuario
                    string query = "UPDATE Usuario SET nombre = @Nombre, apellido = @Apellido, telefono = @Telefono, " +
                                   "ci = @Ci, correo = @Correo, contraseña = @Contraseña WHERE cod = @Cod";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                        cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                        cmd.Parameters.AddWithValue("@Ci", usuario.Ci);
                        cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                        cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                        cmd.Parameters.AddWithValue("@Cod", usuario.Cod);

                        cmd.ExecuteNonQuery();
                    }

                    mensaje = "Usuario actualizado exitosamente";
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                mensaje = "El correo o el CI del usuario ya existen";
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
                    string query = "DELETE FROM Usuario WHERE cod = @cod";
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

        public Usuario GetById(int id)
        {
            Usuario usuario = new Usuario();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT nombre, apellido, telefono, correo, contraseña FROM Usuario WHERE cod = @Id";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read(); // Mueve el lector a la primera fila
                            usuario.Cod = id;
                            usuario.Nombre = rdr["nombre"].ToString();
                            usuario.Apellido = rdr["apellido"].ToString();
                            usuario.Telefono = rdr["telefono"].ToString();
                            usuario.Correo = rdr["correo"].ToString();
                            usuario.Contraseña = rdr["contraseña"].ToString();
                        }
                        else
                        {
                            usuario.Nombre = "No se encontró al Usuario";
                        }
                    }
                }
            }
            catch (Exception)
            {
                usuario.Nombre = "No se encontró al Usuario";
            }
            return usuario;
        }

        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM Usuario";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario()
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
            catch
            {
                lista = new List<Usuario>();
            }

            return lista;
        }

        public Usuario GetByCorreo(string correo)
        {
            Usuario usuario = new Usuario();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT cod, nombre, ci, apellido, telefono, correo, contraseña FROM Usuario WHERE correo = @correo LIMIT 1";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@correo", correo);
                    con.Open();
                    using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read(); // Mueve el lector a la primera fila
                            usuario.Cod = Convert.ToInt32(rdr["cod"]);
                            usuario.Nombre = rdr["nombre"].ToString();
                            usuario.Apellido = rdr["apellido"].ToString();
                            usuario.Telefono = rdr["telefono"].ToString();
                            usuario.Correo = rdr["correo"].ToString();
                            usuario.Contraseña = rdr["contraseña"].ToString();
                            usuario.Ci = rdr["ci"].ToString();
                        }
                        else
                        {
                            usuario = null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                usuario = null;
            }
            return usuario;
        }
    }
}
