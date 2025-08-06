using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Data;

namespace PoliDeportivo.DataAccess
{
    class D_Jornadas
    {
        // Listado de jornadas con información del campeonato
        public DataTable Listado_Jornadas()
        {
            DataTable tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"SELECT
                                    j.pk_jornada_id AS 'Código Jornada',
                                    j.jor_cant_partidos AS 'Cantidad Partidos',
                                    c.pk_campeonato_id AS 'Id campeonato',
                                    c.cam_nombre AS 'Nombre Campeonato'
                                FROM tbl_jornada j
                                INNER JOIN tbl_campeonato c ON j.fk_campeonato_id = c.pk_campeonato_id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        tabla.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener jornadas: " + ex.Message, ex);
            }
            return tabla;
        }

        // Insertar o actualizar jornada
        public string Guardar_Jornada(int nOpcion, Atrib_Jornadas obj)
        {
            string rpta = "";
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
                        sql = @"INSERT INTO tbl_jornada 
                                (pk_jornada_id, jor_cant_partidos, fk_campeonato_id) 
                                VALUES 
                                (@id_jornada, @cant_partidos, @fk_campeonato_id);";
                    }
                    else // UPDATE
                    {
                        accion = "U";
                        sql = @"UPDATE tbl_jornada SET 
                                jor_cant_partidos = @cant_partidos,
                                fk_campeonato_id = @fk_campeonato_id
                            WHERE pk_jornada_id = @id_jornada;";
                    }

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id_jornada", obj.pk_jornada_id);
                    cmd.Parameters.AddWithValue("@cant_partidos", obj.jor_cant_partidos);
                    cmd.Parameters.AddWithValue("@fk_campeonato_id", obj.fk_campeonato_id);

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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 19);
                        cmdBitacora.Parameters.AddWithValue("@operacion", accion);
                        cmdBitacora.Parameters.AddWithValue("@usuarioId", Sesion.UsuarioId);
                        cmdBitacora.Parameters.AddWithValue("@ip", ObtenerIPLocal());

                        cmdBitacora.ExecuteNonQuery();

                        rpta = "OK";
                    }
                    else
                    {
                        rpta = "No se pudo guardar";
                    }
                }
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            return rpta;
        }

        // Eliminar jornada por ID
        public string Eliminar_Jornada(int pk_jornada_id)
        {
            string rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_jornada WHERE pk_jornada_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_jornada_id);
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 19);
                        cmdBitacora.Parameters.AddWithValue("@operacion", "D");
                        cmdBitacora.Parameters.AddWithValue("@usuarioId", Sesion.UsuarioId);
                        cmdBitacora.Parameters.AddWithValue("@ip", ObtenerIPLocal());

                        cmdBitacora.ExecuteNonQuery();

                        rpta = "OK";
                    }
                    else
                    {
                        rpta = "No se pudo guardar";
                    }
                }
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            return rpta;
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
