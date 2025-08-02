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
    public class D_EstadoPartido
    {
        public DataTable Listado_estadopartido()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"
                        SELECT 
                            pk_estado_partido_id AS 'ID Estado Partido',
                            est_descripcion AS 'Estado'
                        FROM tbl_estado_partido;";

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
                throw new Exception("Error al obtener Estado Partidos: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_EstadoPartido(int nOpcion, Atrb_estadopartido obj)
        {
            string Rpta = "";
            string sql = "";

            try
            {
                if (nOpcion != 1 && nOpcion != 2)
                    throw new ArgumentException("Opción no válida. Solo se permite 1 (insertar) o 2 (actualizar).");

                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (nOpcion == 1) // Insertar
                    {
                        sql = @"INSERT INTO tbl_estado_partido (
                                    pk_estado_partido_id,
                                    est_descripcion
                                ) VALUES (
                                    @id,
                                    @descripcion
                                );";

                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", obj.pk_estado_partido_id);
                        cmd.Parameters.AddWithValue("@descripcion", obj.est_descripcion);
                    }
                    else // Actualizar
                    {
                        sql = @"UPDATE tbl_estado_partido SET
                                    est_descripcion = @descripcion
                                WHERE pk_estado_partido_id = @id;";

                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", obj.pk_estado_partido_id);
                        cmd.Parameters.AddWithValue("@descripcion", obj.est_descripcion);
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

        public string Eliminar_est(int pk_estado_partido_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_estado_partido WHERE pk_estado_partido_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_estado_partido_id);
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
