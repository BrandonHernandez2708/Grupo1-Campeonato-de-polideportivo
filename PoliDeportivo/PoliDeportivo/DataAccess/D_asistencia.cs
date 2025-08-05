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
    internal class D_asistencia
    {
        public DataTable Listado_Asist()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "SELECT \r\n" +
                        "pk_asistencia_id AS \"Código Asistencia\",\r\n" +
                        "fk_anotacion_id AS \"Código Anotación\",\r\n" +
                        "fk_jugador_id AS \"Código Jugador\"\r\n" +
                        "FROM tbl_asistencia";

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
                throw new Exception("Error al obtener Asistencias: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Asist(int nOpcion, Atrb_asistencia obj)
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
                        sql = @"INSERT INTO tbl_asistencia (
                            pk_asistencia_id,
                            fk_anotacion_id,
                            fk_jugador_id
                        ) VALUES (
                            @id_asistencia,
                            @id_anotacion,
                            @id_jugador
                        );";

                    }
                    else // UPDATE
                    {
                        sql = @"UPDATE tbl_asistencia SET
                            fk_anotacion_id = @id_anotacion,
                            fk_jugador_id = @id_jugador
                        WHERE pk_asistencia_id = @id_asistencia;";

                    }

                    cmd.CommandText = sql;

                    // Parámetros comunes
                    cmd.Parameters.AddWithValue("@id_asistencia", obj.pk_asistencia_id);
                    cmd.Parameters.AddWithValue("@id_anotacion", obj.fk_anotacion_id);
                    cmd.Parameters.AddWithValue("@id_jugador", obj.fk_jugador_id);

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

        public string Eliminar_Asist(int asistID_pk)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_asistencia WHERE pk_asistencia_id = @id_asistencia;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id_asistencia", asistID_pk);
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
