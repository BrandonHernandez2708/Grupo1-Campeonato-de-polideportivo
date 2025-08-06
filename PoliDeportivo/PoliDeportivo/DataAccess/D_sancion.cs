using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.DataAccess
{
    internal class D_sancion
    {
        public DataTable Listado_san()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "SELECT \r\n" +
                         "pk_sancion_id AS \"Código Sanción\",\r\n" +
                         "san_descripcion AS \"Descripción\",\r\n" +
                         "san_minuto AS \"Minuto\",\r\n" +
                         "fk_partido_id AS \"Código Partido\",\r\n" +
                         "fk_jugador_id AS \"Código Jugador\"\r\n" +
                         "FROM tbl_sancion";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Tabla.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener Sanciones: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_san(int nOpcion, Atrb_sancion obj)
        {
            string Rpta = "";
            string sql = "";
            string accion = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (nOpcion == 1) // INSERT
                    {
                        accion = "I";
                        sql = @"INSERT INTO tbl_sancion (
                            pk_sancion_id,
                            san_descripcion,
                            san_minuto,
                            fk_partido_id,
                            fk_jugador_id
                        ) VALUES (
                            @id_sancion,
                            @descripcion,
                            @minuto,
                            @id_partido,
                            @id_jugador
                        );";

                    }
                    else // UPDATE
                    {
                        accion = "U";
                        sql = @"UPDATE tbl_sancion SET
                            san_descripcion = @descripcion,
                            san_minuto = @minuto,
                            fk_partido_id = @id_partido,
                            fk_jugador_id = @id_jugador
                        WHERE pk_sancion_id = @id_sancion;";

                    }

                    cmd.CommandText = sql;

                    // Parámetros comunes
                    cmd.Parameters.AddWithValue("@id_sancion", obj.pk_sancion_id);
                    cmd.Parameters.AddWithValue("@descripcion", obj.san_descripcion);
                    cmd.Parameters.AddWithValue("@minuto", obj.san_minuto);
                    cmd.Parameters.AddWithValue("@id_partido", obj.fk_partido_id);
                    cmd.Parameters.AddWithValue("@id_jugador", obj.fk_jugador_id);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows >= 1)
                    {
                        // Bitácora
                        MySqlCommand cmdBitacora = new MySqlCommand();
                        cmdBitacora.Connection = conn;
                        cmdBitacora.CommandText = @"INSERT INTO tbl_bitacora (
                                                        fk_entidad_id,
                                                        bit_operacion,
                                                        bit_fecha_hora,
                                                        fk_usuario_id,
                                                        bit_ip
                                                    ) VALUES (
                                                        @entidad,
                                                        @operacion,
                                                        NOW(),
                                                        @usuarioId,
                                                        @ip
                                                    );";

                        cmdBitacora.Parameters.AddWithValue("@entidad", 45);
                        cmdBitacora.Parameters.AddWithValue("@operacion", accion);
                        cmdBitacora.Parameters.AddWithValue("@usuarioId", Sesion.UsuarioId);
                        cmdBitacora.Parameters.AddWithValue("@ip", ObtenerIPLocal());

                        cmdBitacora.ExecuteNonQuery();

                        Rpta = "OK";
                    }
                    else
                    {
                        Rpta = "No se pudo guardar";
                    }
                }
            }
            catch (Exception ex)
            {
                Rpta = ex.Message;
            }
            return Rpta;
        }

        public string Eliminar_san(int sanID_pk)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_sancion WHERE pk_sancion_id = @id_sancion;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id_sancion", sanID_pk);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows >= 1)
                    {
                        // Bitácora
                        MySqlCommand cmdBitacora = new MySqlCommand();
                        cmdBitacora.Connection = conn;
                        cmdBitacora.CommandText = @"INSERT INTO tbl_bitacora (
                                                        fk_entidad_id,
                                                        bit_operacion,
                                                        bit_fecha_hora,
                                                        fk_usuario_id,
                                                        bit_ip
                                                    ) VALUES (
                                                        @entidad,
                                                        @operacion,
                                                        NOW(),
                                                        @usuarioId,
                                                        @ip
                                                    );";

                        cmdBitacora.Parameters.AddWithValue("@entidad", 45);
                        cmdBitacora.Parameters.AddWithValue("@operacion", "D");
                        cmdBitacora.Parameters.AddWithValue("@usuarioId", Sesion.UsuarioId);
                        cmdBitacora.Parameters.AddWithValue("@ip", ObtenerIPLocal());

                        cmdBitacora.ExecuteNonQuery();
                        Rpta = "OK";
                    }
                    else
                    {
                        Rpta = "No se pudo eliminar";
                    }
                }
            }
            catch (Exception ex)
            {
                Rpta = ex.Message;
            }
            return Rpta;
        }

        private string ObtenerIPLocal()
        {
            string ip = "";
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ipAddr in host.AddressList)
                {
                    if (ipAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ip = ipAddr.ToString();
                        break;
                    }
                }
            }
            catch
            {
                ip = "No disponible";
            }
            return ip;
        }
    }
}
