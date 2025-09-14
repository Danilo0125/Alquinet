using Alquinet_Entidad;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Alquinet_Datos
{
    public class FotoRepository
    {
        public List<Fotos> GetFotosByCod(int cod_propiedad)
        {
            List<Fotos> fotos = new List<Fotos>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();
                    // En PostgreSQL, se usa LIMIT en lugar de TOP
                    string query = "SELECT * FROM Fotos WHERE cod_propiedad = @cod_propiedad LIMIT 10";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@cod_propiedad", cod_propiedad);
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            fotos.Add(new Fotos()
                            {
                                Cod = Convert.ToInt32(dr["cod"]),
                                Ambiente = dr["ambiente"].ToString(),
                                Cod_propiedad = cod_propiedad,
                                DireccionFoto = dr["direccionFoto"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                fotos = null;
            }
            return fotos;
        }

        public int Registrar(Fotos foto, out string mensaje)
        {
            int returnId = -1;
            mensaje = string.Empty;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(Conexion.cn))
                {
                    con.Open();
                    // En PostgreSQL, se usa RETURNING para obtener el ID generado
                    string query = "INSERT INTO Fotos (Ambiente, DireccionFoto, Cod_propiedad) " +
                                   "VALUES (@Ambiente, @DireccionFoto, @Cod_propiedad) " +
                                   "RETURNING Cod";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Ambiente", foto.Ambiente);
                        cmd.Parameters.AddWithValue("@DireccionFoto", foto.DireccionFoto);
                        cmd.Parameters.AddWithValue("@Cod_propiedad", foto.Cod_propiedad);

                        returnId = (int)cmd.ExecuteScalar();
                    }
                    mensaje = "Foto guardada exitosamente";
                }
            }
            catch (PostgresException ex)
            {
                mensaje = ex.Message;
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
            }

            return returnId;
        }
    }
}
