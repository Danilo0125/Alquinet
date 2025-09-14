using Alquinet_Entidad;
using Alquinet_Entidad.ClasesPropiedad;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Alquinet_Datos
{
    public class PropiedadDatos
    {
        public int Registrar(Propiedad propiedad, out string mensaje)
        {
            int returnId = -1;
            mensaje = string.Empty;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();
                    string query = "INSERT INTO Propiedad (titulo, area, tipo, descripcion, disponibilidad, precio, cod_usuario, cod_direccion) " +
                                   "VALUES (@titulo, @area, @tipo, @descripcion, @disponibilidad, @precio, @cod_usuario, @cod_direccion) " +
                                   "RETURNING cod";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@titulo", propiedad.Titulo);
                        cmd.Parameters.AddWithValue("@area", propiedad.Area);
                        cmd.Parameters.AddWithValue("@tipo", propiedad.Tipo);
                        cmd.Parameters.AddWithValue("@descripcion", propiedad.Descripcion);
                        cmd.Parameters.AddWithValue("@disponibilidad", propiedad.Disponibilidad);
                        cmd.Parameters.AddWithValue("@precio", propiedad.Precio);
                        cmd.Parameters.AddWithValue("@cod_usuario", propiedad.Cod_usuario);
                        cmd.Parameters.AddWithValue("@cod_direccion", propiedad.Cod_direccion);

                        returnId = (int)cmd.ExecuteScalar();
                    }
                    mensaje = "Propiedad creada exitosamente";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
            }

            return returnId;
        }


        public bool Editar(Propiedad propiedad, out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();
                    string query = "UPDATE Propiedad SET titulo = @titulo, area = @area, tipo = @tipo, " +
                                   "descripcion = @descripcion, disponibilidad = @disponibilidad, precio = @precio, " +
                                   "cod_usuario = @cod_usuario, cod_agente = @cod_agente, cod_direccion = @cod_direccion " +
                                   "WHERE cod = @cod";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@titulo", propiedad.Titulo);
                        cmd.Parameters.AddWithValue("@area", propiedad.Area);
                        cmd.Parameters.AddWithValue("@tipo", propiedad.Tipo);
                        cmd.Parameters.AddWithValue("@descripcion", propiedad.Descripcion);
                        cmd.Parameters.AddWithValue("@disponibilidad", propiedad.Disponibilidad);
                        cmd.Parameters.AddWithValue("@precio", propiedad.Precio);
                        cmd.Parameters.AddWithValue("@cod_usuario", propiedad.Cod_usuario);
                        cmd.Parameters.AddWithValue("@cod_agente", propiedad.Cod_agente);
                        cmd.Parameters.AddWithValue("@cod_direccion", propiedad.Cod_direccion);
                        cmd.Parameters.AddWithValue("@cod", propiedad.Cod);
                        cmd.ExecuteNonQuery();
                    }

                    mensaje = "Propiedad actualizada exitosamente";
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
                    string query = "DELETE FROM Propiedad WHERE cod = @cod";
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

        public Propiedad GetById(int id)
        {
            Propiedad propiedad = new Propiedad();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT titulo, area, tipo, descripcion, disponibilidad, precio, cod_usuario, cod_agente, cod_direccion " +
                                   "FROM Propiedad WHERE cod = @Id";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            propiedad.Cod = id;
                            propiedad.Titulo = dr["titulo"].ToString();
                            propiedad.Area = Convert.ToDecimal(dr["area"]);
                            propiedad.Tipo = dr["tipo"].ToString();
                            propiedad.Descripcion = dr["descripcion"].ToString();
                            propiedad.Disponibilidad = dr["disponibilidad"].ToString();
                            propiedad.Precio = Convert.ToDecimal(dr["precio"]);
                            propiedad.Cod_usuario = Convert.ToInt32(dr["cod_usuario"]);
                            propiedad.Cod_agente = dr["cod_agente"] != DBNull.Value ? Convert.ToInt32(dr["cod_agente"]) : 0; // Asignar 0 o manejar como prefieras
                            propiedad.Cod_direccion = Convert.ToInt32(dr["cod_direccion"]);
                        }
                    }
                }
            }
            catch (Exception)
            {
                propiedad = null;
            }
            return propiedad;
        }

        public List<Propiedad> Listar()
        {
            List<Propiedad> lista = new List<Propiedad>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM Propiedad";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Propiedad propiedad = new Propiedad()
                            {
                                Cod = Convert.ToInt32(dr["cod"]),
                                Titulo = dr["titulo"].ToString(),
                                Area = Convert.ToDecimal(dr["area"]),
                                Tipo = dr["tipo"].ToString(),
                                Descripcion = dr["descripcion"].ToString(),
                                Disponibilidad = dr["disponibilidad"].ToString(),
                                Precio = Convert.ToDecimal(dr["precio"]),
                                Cod_usuario = Convert.ToInt32(dr["cod_usuario"]),
                                Cod_direccion = Convert.ToInt32(dr["cod_direccion"])
                            };
                            try
                            {
                                propiedad.Cod_agente = Convert.ToInt32(dr["cod_agente"]);
                            }
                            catch { }
                            lista.Add(propiedad);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                lista = new List<Propiedad>();
            }

            return lista;
        }

        public List<Propiedad> ListarDisponibles()
        {
            List<Propiedad> lista = new List<Propiedad>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM Propiedad WHERE cod_agente IS NOT NULL";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Propiedad propiedad = new Propiedad()
                            {
                                Cod = Convert.ToInt32(dr["cod"]),
                                Titulo = dr["titulo"].ToString(),
                                Area = Convert.ToDecimal(dr["area"]),
                                Tipo = dr["tipo"].ToString(),
                                Descripcion = dr["descripcion"].ToString(),
                                Disponibilidad = dr["disponibilidad"].ToString(),
                                Precio = Convert.ToDecimal(dr["precio"]),
                                Cod_usuario = Convert.ToInt32(dr["cod_usuario"]),
                                Cod_direccion = Convert.ToInt32(dr["cod_direccion"])
                            };
                            try
                            {
                                propiedad.Cod_agente = Convert.ToInt32(dr["cod_agente"]);
                            }
                            catch { }
                            lista.Add(propiedad);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                lista = new List<Propiedad>();
            }
            return lista;
        }
    }
}
