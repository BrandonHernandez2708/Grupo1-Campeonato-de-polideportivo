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
    internal class D_Contratacion
    {
        public DataTable Listado_Contratacion()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "SELECT \r\n" +
                              "fk_puesto_id AS \"Código Puesto\",\r\n" +
                              "fk_empleado_id AS \"Código Empleado\",\r\n" +
                              "con_fecha AS \"Fecha de la operación\",\r\n" +
                              "CASE con_tipo_operacion\r\n" +
                              "    WHEN 0 THEN 'Contratación'\r\n" +
                              "    WHEN 1 THEN 'Cambio de Puesto'\r\n" +
                              "    WHEN 2 THEN 'Despido'\r\n" +
                              "    ELSE 'Desconocido'\r\n" +
                              "    END AS \"Tipo de operación\"\r\n" +
                                "FROM tbl_contratacion;\r\n";
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
                throw new Exception("Error al obtener deportes: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Con(int nOpcion, Atrb_Contratacion obj)
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

                    if (nOpcion == 1) // Insertar
                    {
                        accion = "I";
                        sql = @"INSERT INTO tbl_contratacion (
                                    fk_puesto_id,
                                    fk_empleado_id,
                                    con_fecha,
                                    con_tipo_operacion
                                ) VALUES (
                                    @puestoId,
                                    @empleadoId,
                                    @fecha,
                                    @tipoOperacion
                                );
                                ";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@puestoId", obj.fk_puesto_id);
                        cmd.Parameters.AddWithValue("@empleadoId", obj.fk_empleado_id);
                        cmd.Parameters.AddWithValue("@fecha", obj.con_fecha);
                        cmd.Parameters.AddWithValue("@tipoOperacion", obj.con_tipo_operacion);
                    }
                    else // Actualizar
                    {
                        accion = "U";
                        sql = @"UPDATE tbl_contratacion SET
                                    con_fecha = @fecha,
                                    con_tipo_operacion = @tipoOperacion
                                WHERE fk_puesto_id = @puestoId AND fk_empleado_id = @empleadoId;";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@puestoId", obj.fk_puesto_id);
                        cmd.Parameters.AddWithValue("@empleadoId", obj.fk_empleado_id);
                        cmd.Parameters.AddWithValue("@fecha", obj.con_fecha);
                        cmd.Parameters.AddWithValue("@tipoOperacion", obj.con_tipo_operacion);
                    }

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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 18);
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

        public string Eliminar_Con(int fk_puesto_id, int fk_empleado_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_contratacion\r\n" +
                        "WHERE fk_puesto_id = @puestoId AND fk_empleado_id = @empleadoId;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@puestoId", fk_puesto_id);
                    cmd.Parameters.AddWithValue("@empleadoId", fk_empleado_id);
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 18);
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
