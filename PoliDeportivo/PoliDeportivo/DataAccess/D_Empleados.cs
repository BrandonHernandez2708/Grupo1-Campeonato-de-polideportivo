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
    internal class D_Empleados
    {
        public DataTable Listado_Emp()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "SELECT \r\n" +
                       "pk_empleado_id AS \"Código Empleado\",\r\n" +
                       "emp_nombre AS \"Nombre\",\r\n" +
                       "emp_apellido AS \"Apellido\"\r\n" +
                       "from tbl_empleado";
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
                throw new Exception("Error al obtener Empleados: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Emp(int nOpcion, Atrb_Empleados obj)
        {
            string Rpta = "";
            string sql = "";

            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (nOpcion == 1) // INSERT
                    {
                        sql = @"INSERT INTO tbl_empleado (
                            pk_empleado_id,
                            emp_nombre,
                            emp_apellido
                        ) VALUES (
                            @id_empleado,
                            @nombre,
                            @apellido
                        );";
                    }
                    else // UPDATE
                    {
                        sql = @"UPDATE tbl_empleado SET
                            emp_nombre = @nombre,
                            emp_apellido = @apellido
                        WHERE pk_empleado_id = @id_empleado;";
                    }

                    cmd.CommandText = sql;

                    // Parámetros comunes
                    cmd.Parameters.AddWithValue("@id_empleado", obj.pk_empleado_id);
                    cmd.Parameters.AddWithValue("@nombre", obj.emp_nombre);
                    cmd.Parameters.AddWithValue("@apellido", obj.emp_apellido);

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

        public string Eliminar_Emp(int empID_pk)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_empleado WHERE pk_empleado_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", empID_pk);
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
