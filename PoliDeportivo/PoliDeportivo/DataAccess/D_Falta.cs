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
    internal class D_Falta
    {
        public DataTable Listado_Falta()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"
                        SELECT
                            f.pk_falta_id        AS 'ID Falta',
                            f.fal_descripcion    AS 'Descripción',
                            f.fal_minuto         AS 'Minuto',
                            p.pk_partido_id      AS 'ID Partido',
                            j.pk_jugador_id      AS 'ID Jugador'
                        FROM tbl_falta f
                        INNER JOIN tbl_partido p  ON f.fk_partido_id = p.pk_partido_id
                        INNER JOIN tbl_jugador j  ON f.fk_jugador_id = j.pk_jugador_id
                        ORDER BY f.pk_falta_id;
                        ";
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
                throw new Exception("Error al obtener faltas: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Falta(int nOpcion, Atrb_Falta obj)
        {
            string Rpta = "";
            string sql;
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
                        sql = @"
                            INSERT INTO tbl_falta (
                                fal_descripcion,
                                fal_minuto,
                                fk_partido_id,
                                fk_jugador_id
                            ) VALUES (
                                @descripcion,
                                @minuto,
                                @fk_partido_id,
                                @fk_jugador_id
                            );";
                    }
                    else // UPDATE
                    {
                        accion = "U";
                        sql = @"
                            UPDATE tbl_falta SET
                                fal_descripcion = @descripcion,
                                fal_minuto      = @minuto,
                                fk_partido_id   = @fk_partido_id,
                                fk_jugador_id   = @fk_jugador_id
                            WHERE pk_falta_id = @id;";
                    }

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id", obj.pk_falta_id);
                    cmd.Parameters.AddWithValue("@descripcion", obj.fal_descripcion);
                    cmd.Parameters.AddWithValue("@minuto", obj.fal_minuto);
                    cmd.Parameters.AddWithValue("@fk_partido_id", obj.fk_partido_id);
                    cmd.Parameters.AddWithValue("@fk_jugador_id", obj.fk_jugador_id);



                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows >= 1)
                    {
                        // Insertar en bitácora
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 21);
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


        public string Eliminar_Falta(int pk_falta_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_falta WHERE pk_falta_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_falta_id);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows >= 1)
                    {
                        // Insertar en bitácora
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 21);
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
