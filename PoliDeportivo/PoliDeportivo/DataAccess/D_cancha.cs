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
                    string sql = "SELECT \r\n" +
                     "pk_cancha_id AS \"Código Cancha\",\r\n" +
                     "can_capacidad AS \"Capacidad\",\r\n" +
                     "can_direccion AS \"Dirección\"\r\n" +
                     "FROM tbl_cancha;\r\n";

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

        public string Guardar_cancha(int nOpcion, Atrb_cancha obj)
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
                         sql = @"INSERT INTO tbl_cancha (
                                pk_cancha_id,
                                can_capacidad,
                                can_direccion
                            ) VALUES (
                                @id,
                                @capacidad,
                                @direccion
                            );";

                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", obj.pk_cancha_id);
                        cmd.Parameters.AddWithValue("@capacidad", obj.can_capacidad);
                        cmd.Parameters.AddWithValue("@direccion", obj.can_direccion);

                    }
                    else // Actualizar
                    {
                        sql = @"UPDATE tbl_cancha SET
                                can_capacidad = @capacidad,
                                can_direccion = @direccion
                            WHERE pk_cancha_id = @id;";

                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", obj.pk_cancha_id);
                        cmd.Parameters.AddWithValue("@capacidad", obj.can_capacidad);
                        cmd.Parameters.AddWithValue("@direccion", obj.can_direccion);
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