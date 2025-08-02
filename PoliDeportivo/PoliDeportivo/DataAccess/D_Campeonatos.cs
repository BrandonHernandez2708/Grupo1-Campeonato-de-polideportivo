using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Data;

namespace PoliDeportivo.DataAccess
{
    public class D_Campeonatos
    {
        public DataTable Listado_Campeonatos()
        {
            DataTable tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"
                            SELECT 
                                e.pk_campeonato_id AS 'Id campeonato',
                                e.cam_nombre AS 'Nombre',
                                e.cam_modalidad AS 'Modalidad',
                                e.cam_cant_equipos AS 'Cantidad equipos',
                                e.cam_fecha_inicio AS 'Fecha inicio',
                                e.cam_fecha_final AS 'Fecha final',
                                c.jor_cant_partidos AS 'Cantidad partidos'
                            FROM tbl_campeonato e
                            LEFT JOIN tbl_jornada c ON e.pk_campeonato_id = c.fk_campeonato_id";
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
                throw new Exception("Error al obtener campeonatos: " + ex.Message, ex);
            }
            return tabla;
        }

        public string Guardar_Campeonato(int nOpcion, Atrb_campeonatos obj)
        {
            string respuesta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    conn.Open();
                    using (MySqlTransaction trans = conn.BeginTransaction())
                    {
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = trans;

                        try
                        {
                            if (nOpcion == 1) // Insertar
                            {
                                cmd.CommandText = @"INSERT INTO tbl_campeonato (pk_campeonato_id, cam_nombre, cam_modalidad, cam_cant_equipos, cam_fecha_inicio, cam_fecha_final) 
                                                        VALUES (@id, @nombre, @modalidad, @cantidad_equipos, @fecha_inicio, @fecha_final)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@id", obj.pk_campeonato_id);
                                cmd.Parameters.AddWithValue("@nombre", obj.cam_nombre);
                                cmd.Parameters.AddWithValue("@modalidad", obj.cam_modalidad);
                                cmd.Parameters.AddWithValue("@cantidad_equipos", obj.cam_cant_equipos);
                                cmd.Parameters.AddWithValue("@fecha_inicio", obj.cam_fecha_inicio);
                                cmd.Parameters.AddWithValue("@fecha_final", obj.cam_fecha_final);
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = @"INSERT INTO tbl_jornada (pk_jornada_id, jor_cant_partidos, fk_campeonato_id) 
                                                        VALUES (@pk_j_id, @jor_cant_p, @fk_Camp_id)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@pk_j_id", GenerarNuevoIDJornada(conn, trans));
                                cmd.Parameters.AddWithValue("@jor_cant_p", obj.jor_cant_partidos);
                                cmd.Parameters.AddWithValue("@fk_Camp_id", obj.pk_campeonato_id);
                                cmd.ExecuteNonQuery();
                            }
                            else if (nOpcion == 2) // Actualizar
                            {
                                cmd.CommandText = @"UPDATE tbl_campeonato SET cam_nombre = @nombre, cam_modalidad = @modalidad, cam_cant_equipos = @cantidad_equipos, cam_fecha_inicio = @fecha_inicio, cam_fecha_final = @fecha_final
                                                        WHERE pk_campeonato_id = @id";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@nombre", obj.cam_nombre);
                                cmd.Parameters.AddWithValue("@modalidad", obj.cam_modalidad);
                                cmd.Parameters.AddWithValue("@cantidad_equipos", obj.cam_cant_equipos);
                                cmd.Parameters.AddWithValue("@fecha_inicio", obj.cam_fecha_inicio);
                                cmd.Parameters.AddWithValue("@fecha_final", obj.cam_fecha_final);
                                cmd.Parameters.AddWithValue("@id", obj.pk_campeonato_id);
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = @"UPDATE tbl_jornada SET jor_cant_partidos = @jor_cant_p WHERE fk_campeonato_id = @fk_Camp_id";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@jor_cant_p", obj.jor_cant_partidos);
                                cmd.Parameters.AddWithValue("@fk_Camp_id", obj.pk_campeonato_id);
                                cmd.ExecuteNonQuery();
                            }

                            trans.Commit();
                            respuesta = "OK";
                        }
                        catch (Exception exTrans)
                        {
                            trans.Rollback();
                            respuesta = "Error al guardar/actualizar: " + exTrans.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error en conexión: " + ex.Message;
            }

            return respuesta;
        }

        public string Eliminar_Campeonato(int pk_campeonato_id)
        {
            string respuesta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    conn.Open();
                    using (MySqlTransaction trans = conn.BeginTransaction())
                    {
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = trans;

                        try
                        {
                            cmd.CommandText = "DELETE FROM tbl_jornada WHERE fk_campeonato_id = @id";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", pk_campeonato_id);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "DELETE FROM tbl_campeonato WHERE pk_campeonato_id = @id";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", pk_campeonato_id);
                            int filas = cmd.ExecuteNonQuery();

                            if (filas >= 1)
                                respuesta = "OK";
                            else
                                respuesta = "No se pudo eliminar";

                            trans.Commit();
                        }
                        catch (Exception exTrans)
                        {
                            trans.Rollback();
                            respuesta = "Error al eliminar: " + exTrans.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error en conexión: " + ex.Message;
            }
            return respuesta;
        }

        private int GenerarNuevoIDJornada(MySqlConnection conn, MySqlTransaction trans)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT IFNULL(MAX(pk_jornada_id), 0) + 1 FROM tbl_jornada", conn, trans);
            int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
            return nuevoId;
        }
    }
}
