using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Data;

namespace PoliDeportivo.DataAccess
{
    public class D_Entrenadores
    {
        public DataTable Listado_Entrenadores()
        {
            DataTable tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"
                        SELECT 
                            e.pk_entrenador_id AS 'id_entrenador',
                            e.ent_nombre AS 'nombre',
                            e.ent_apellido AS 'apellido',
                            t.tel_numero AS 'telefono',
                            c.correo AS 'correo'
                        FROM tbl_entrenador e
                        LEFT JOIN tbl_telefono_entrenador t ON e.pk_entrenador_id = t.fk_entrenador_id
                        LEFT JOIN tbl_correo_entrenador c ON e.pk_entrenador_id = c.fk_entrenador_id";

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
                throw new Exception("Error al obtener entrenadores: " + ex.Message, ex);
            }
            return tabla;
        }

        public string Guardar_Entrenador(int nOpcion, Atrb_entrenadores obj)
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
                            if (nOpcion == 1) // Nuevo
                            {
                                cmd.CommandText = @"INSERT INTO tbl_entrenador (pk_entrenador_id, ent_nombre, ent_apellido) 
                                                    VALUES (@id, @nombre, @apellido)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@id", obj.pk_entrenador_id);
                                cmd.Parameters.AddWithValue("@nombre", obj.ent_nombre);
                                cmd.Parameters.AddWithValue("@apellido", obj.ent_apellido);
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = @"INSERT INTO tbl_telefono_entrenador (pk_tel_entrenador_id, tel_numero, fk_entrenador_id) 
                                                    VALUES (@telId, @telefono, @id_fk)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@telId", GenerarNuevoIDTelefono(conn, trans));
                                cmd.Parameters.AddWithValue("@telefono", obj.tel_numero);
                                cmd.Parameters.AddWithValue("@id_fk", obj.pk_entrenador_id);
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = @"INSERT INTO tbl_correo_entrenador (pk_cor_entrenador_id, correo, fk_entrenador_id) 
                                                    VALUES (@corId, @correo, @id_fk)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@corId", GenerarNuevoIDCorreo(conn, trans));
                                cmd.Parameters.AddWithValue("@correo", obj.correo);
                                cmd.Parameters.AddWithValue("@id_fk", obj.pk_entrenador_id);
                                cmd.ExecuteNonQuery();
                            }
                            else if (nOpcion == 2) // Actualizar
                            {
                                cmd.CommandText = @"UPDATE tbl_entrenador SET ent_nombre = @nombre, ent_apellido = @apellido 
                                                    WHERE pk_entrenador_id = @id";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@nombre", obj.ent_nombre);
                                cmd.Parameters.AddWithValue("@apellido", obj.ent_apellido);
                                cmd.Parameters.AddWithValue("@id", obj.pk_entrenador_id);
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = @"SELECT COUNT(*) FROM tbl_telefono_entrenador WHERE fk_entrenador_id = @id";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@id", obj.pk_entrenador_id);
                                int countTel = Convert.ToInt32(cmd.ExecuteScalar());

                                if (countTel > 0)
                                {
                                    cmd.CommandText = @"UPDATE tbl_telefono_entrenador SET tel_numero = @telefono WHERE fk_entrenador_id = @id";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@telefono", obj.tel_numero);
                                    cmd.Parameters.AddWithValue("@id", obj.pk_entrenador_id);
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    cmd.CommandText = @"INSERT INTO tbl_telefono_entrenador (pk_tel_entrenador_id, tel_numero, fk_entrenador_id) 
                                                       VALUES (@telId, @telefono, @id)";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@telId", GenerarNuevoIDTelefono(conn, trans));
                                    cmd.Parameters.AddWithValue("@telefono", obj.tel_numero);
                                    cmd.Parameters.AddWithValue("@id", obj.pk_entrenador_id);
                                    cmd.ExecuteNonQuery();
                                }

                                cmd.CommandText = @"SELECT COUNT(*) FROM tbl_correo_entrenador WHERE fk_entrenador_id = @id";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@id", obj.pk_entrenador_id);
                                int countCor = Convert.ToInt32(cmd.ExecuteScalar());

                                if (countCor > 0)
                                {
                                    cmd.CommandText = @"UPDATE tbl_correo_entrenador SET correo = @correo WHERE fk_entrenador_id = @id";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@correo", obj.correo);
                                    cmd.Parameters.AddWithValue("@id", obj.pk_entrenador_id);
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    cmd.CommandText = @"INSERT INTO tbl_correo_entrenador (pk_cor_entrenador_id, correo, fk_entrenador_id) 
                                                       VALUES (@corId, @correo, @id)";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@corId", GenerarNuevoIDCorreo(conn, trans));
                                    cmd.Parameters.AddWithValue("@correo", obj.correo);
                                    cmd.Parameters.AddWithValue("@id", obj.pk_entrenador_id);
                                    cmd.ExecuteNonQuery();
                                }
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

        public string Eliminar_Entrenador(int pk_entrenador_id)
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
                            cmd.CommandText = "DELETE FROM tbl_telefono_entrenador WHERE fk_entrenador_id = @id";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", pk_entrenador_id);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "DELETE FROM tbl_correo_entrenador WHERE fk_entrenador_id = @id";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", pk_entrenador_id);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "DELETE FROM tbl_entrenador WHERE pk_entrenador_id = @id";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", pk_entrenador_id);
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

        private int GenerarNuevoIDTelefono(MySqlConnection conn, MySqlTransaction trans)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT IFNULL(MAX(pk_tel_entrenador_id), 0) + 1 FROM tbl_telefono_entrenador", conn, trans);
            int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
            return nuevoId;
        }

        private int GenerarNuevoIDCorreo(MySqlConnection conn, MySqlTransaction trans)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT IFNULL(MAX(pk_cor_entrenador_id), 0) + 1 FROM tbl_correo_entrenador", conn, trans);
            int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
            return nuevoId;
        }
    }
}
