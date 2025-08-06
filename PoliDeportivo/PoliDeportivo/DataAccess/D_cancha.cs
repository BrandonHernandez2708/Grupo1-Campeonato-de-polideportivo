using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Data;

namespace PoliDeportivo.DataAccess
{
    public class D_canchas
    {
        public DataTable Listado_Can()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"
                        SELECT 
                            pk_cancha_id AS 'Código Cancha',
                            can_capacidad AS 'Capacidad',
                            can_direccion AS 'Dirección'
                        FROM tbl_cancha;";
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
                throw new Exception("Error al obtener canchas: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_cancha(int nOpcion, Atrb_cancha obj)
        {
            string Rpta = "";
            string sql = "";
            string accion = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (nOpcion == 1) // Insertar
                    {
                        accion = "I";
                        sql = @"INSERT INTO tbl_cancha (
                                    pk_cancha_id,
                                    can_capacidad,
                                    can_direccion
                                ) VALUES (
                                    @id,
                                    @capacidad,
                                    @direccion
                                );";
                    }
                    else // Actualizar
                    {
                        accion = "U";
                        sql = @"UPDATE tbl_cancha SET
                                    can_capacidad = @capacidad,
                                    can_direccion = @direccion
                                WHERE pk_cancha_id = @id;";
                    }

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id", obj.pk_cancha_id);
                    cmd.Parameters.AddWithValue("@capacidad", obj.can_capacidad);
                    cmd.Parameters.AddWithValue("@direccion", obj.can_direccion);

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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 8); 
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

        public string Eliminar_Can(int pk_cancha_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_cancha WHERE pk_cancha_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_cancha_id);
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 8);
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
