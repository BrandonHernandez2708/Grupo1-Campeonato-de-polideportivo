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
    internal class D_Tel_Empleado
    {
        public DataTable Listado_Tel()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "SELECT \r\n" +
                     "tel.pk_tel_empleado_id AS \"ID Teléfono\",\r\n" +
                     "tel.tel_empleado AS \"Número\",\r\n" +
                     "tel.fk_empleado_id AS \"ID Empleado\",\r\n" +
                     "emp.emp_nombre AS \"Nombre Dueño\",\r\n" +
                     "emp.emp_apellido AS \"Apellido Dueño\"\r\n" +
                     "FROM tbl_telefono_empleado AS tel\r\n" +
                 "INNER JOIN tbl_empleado AS emp ON tel.fk_empleado_id = emp.pk_empleado_id;\r\n";

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
                throw new Exception("Error al obtener Telefonos: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Tel(int nOpcion, Atrb_Tel_Emp obj)
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
                            sql = @"INSERT INTO tbl_telefono_empleado (
                                    pk_tel_empleado_id,
                                    tel_empleado,
                                    fk_empleado_id
                                ) VALUES (
                                    @id,
                                    @numero,
                                    @empleadoId
                                );";

                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", obj.pk_tel_empleado_id);
                        cmd.Parameters.AddWithValue("@numero", obj.tel_empleado);
                        cmd.Parameters.AddWithValue("@empleadoId", obj.fk_empleado_id);

                    }
                    else // Actualizar
                    {
                        accion = "U";
                        sql = @"UPDATE tbl_telefono_empleado SET
                                    tel_empleado = @numero,
                                    fk_empleado_id = @empleadoId
                                WHERE pk_tel_empleado_id = @id;";

                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", obj.pk_tel_empleado_id);
                        cmd.Parameters.AddWithValue("@numero", obj.tel_empleado);
                        cmd.Parameters.AddWithValue("@empleadoId", obj.fk_empleado_id);
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 14);
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

        public string Eliminar_Tel_Emp(int pk_Tel_Emp_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_telefono_empleado WHERE pk_tel_empleado_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_Tel_Emp_id);
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 14);
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
