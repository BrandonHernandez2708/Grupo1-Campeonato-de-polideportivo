using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Data;

namespace PoliDeportivo.DataAccess
{
    class D_Anotaciones
    {
        public DataTable Listado_Anotaciones()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"SELECT
                                    e.pk_anotacion_id AS 'ID anotación',
                                    e.ano_minuto AS 'Minuto',
                                    e.ano_descripcion AS 'Descripcion',
                                    e.fk_partido_id AS 'ID partido',
                                    e.fk_jugador_id AS 'ID jugador'
                                FROM tbl_anotacion e
                                INNER JOIN tbl_partido p ON e.fk_partido_id = p.pk_partido_id
                                INNER JOIN tbl_jugador j ON e.fk_jugador_id = j.pk_jugador_id;";

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
                throw new Exception("Error al obtener anotaciones: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Anotacion(int nOpcion, Atrb_Anotaciones obj)
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
                        sql = @"INSERT INTO tbl_anotacion 
                                (pk_anotacion_id, ano_minuto, ano_descripcion, fk_partido_id, fk_jugador_id) 
                                VALUES 
                                (@id_anotacion, @minuto, @descrip, @id_partido, @id_jugador);";
                    }
                    else // UPDATE
                    {
                        accion = "U";
                        sql = @"UPDATE tbl_anotacion SET 
                                ano_minuto = @minuto,
                                ano_descripcion = @descrip,
                                fk_partido_id = @id_partido,
                                fk_jugador_id = @id_jugador
                                WHERE pk_anotacion_id = @id_anotacion;";
                    }

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id_anotacion", obj.pk_anotacion_id);
                    cmd.Parameters.AddWithValue("@minuto", obj.ano_minuto);
                    cmd.Parameters.AddWithValue("@descrip", obj.ano_descripcion);
                    cmd.Parameters.AddWithValue("@id_partido", obj.fk_partido_id);
                    cmd.Parameters.AddWithValue("@id_jugador", obj.fk_jugador_id);

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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 23);
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

        public string Eliminar_Anotacion(int pk_anotacion_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_anotacion WHERE pk_anotacion_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_anotacion_id);
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 23);
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
