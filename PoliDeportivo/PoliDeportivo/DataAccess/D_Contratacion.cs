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
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (nOpcion == 1) // Insertar
                    {
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
                    Rpta = rows >= 1 ? "OK" : "No se pudo guardar";
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
                    Rpta = rows >= 1 ? "OK" : "No se pudo eliminar";
                }
            }
            catch (Exception ex)
            {
                Rpta = ex.Message;
            }
            return Rpta;
        }
    }
}
