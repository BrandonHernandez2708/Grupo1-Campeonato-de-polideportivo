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
    class D_Puesto
    {
        public DataTable Listado_Puesto()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "SELECT \r\n" +
                            "pk_puesto_id AS \"Código Puesto\",\r\n" +
                            "pue_nombre AS \"Nombre del Puesto\",\r\n" +
                            "pue_descripcion AS \"Descripción\"\r\n" +
                            "FROM tbl_puesto";
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
                throw new Exception("Error al obtener Puestos: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Puesto(int nOpcion, Atrb_Puesto obj)
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
                       sql = @"INSERT INTO tbl_puesto (
                                        pk_puesto_id,
                                        pue_nombre,
                                        pue_descripcion
                                    ) VALUES (
                                        @id_puesto,
                                        @nombre,
                                        @descripcion
                                    );";
                    }
                    else // UPDATE
                    {
                         sql = @"UPDATE tbl_puesto SET
                                pue_nombre = @nombre,
                                pue_descripcion = @descripcion
                            WHERE pk_puesto_id = @id_puesto;";
                    }

                    cmd.CommandText = sql;

                    // Parámetros comunes
                    cmd.Parameters.AddWithValue("@id_puesto", obj.pk_puesto_id);
                    cmd.Parameters.AddWithValue("@nombre", obj.pue_nombre);
                    cmd.Parameters.AddWithValue("@descripcion", obj.pue_descripcion);

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

        public string Eliminar_puesto(int pk_puesto_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_puesto WHERE pk_puesto_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_puesto_id);
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
