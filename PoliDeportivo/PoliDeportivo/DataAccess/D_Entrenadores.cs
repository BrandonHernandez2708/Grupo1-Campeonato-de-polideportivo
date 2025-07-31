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
                using (MySqlConnection conn = conexionmysql.geInstancia().CrearConexion())
                {
                    string sql = @"
                        SELECT 
                            e.entID_pk AS id_entrenador,
                            e.entNOMBRE AS nombre,
                            e.entAPELLIDO AS apellido,
                            t.tel_entrenador AS telefono,
                            c.Cor_entrenador AS correo
                        FROM tblENTRENADORES e
                        LEFT JOIN tblTELEFONO_ENTRENADOR t ON e.entID_pk = t.entID_fk
                        LEFT JOIN tblCORREO_ENTRENADOR c ON e.entID_pk = c.entID_fk";

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
                using (MySqlConnection conn = conexionmysql.geInstancia().CrearConexion())
                {
                    conn.Open();
                    using (MySqlTransaction trans = conn.BeginTransaction())
                    {
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = trans;

                        try
                        {
                            if (nOpcion == 1) 
                            {

                                cmd.CommandText = @"INSERT INTO tblENTRENADORES (entID_pk, entNOMBRE, entAPELLIDO) 
                                                    VALUES (@id, @nombre, @apellido)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@id", obj.entID_pk);
                                cmd.Parameters.AddWithValue("@nombre", obj.entNOMBRE);
                                cmd.Parameters.AddWithValue("@apellido", obj.entAPELLIDO);
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = @"INSERT INTO tblTELEFONO_ENTRENADOR (tel_entrenadorID_pk, tel_entrenador, entID_fk) 
                                                    VALUES (@telId, @telefono, @id_fk)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@telId", GenerarNuevoIDTelefono(conn, trans)); // Método para obtener nuevo tel_entrenadorID_pk manual
                                cmd.Parameters.AddWithValue("@telefono", obj.tel_entrenador);
                                cmd.Parameters.AddWithValue("@id_fk", obj.entID_pk);
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = @"INSERT INTO tblCORREO_ENTRENADOR (cor_entrenadorID_pk, Cor_entrenador, entID_fk) 
                                                    VALUES (@corId, @correo, @id_fk)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@corId", GenerarNuevoIDCorreo(conn, trans)); // Método para obtener nuevo cor_entrenadorID_pk manual
                                cmd.Parameters.AddWithValue("@correo", obj.cor_entrenador);
                                cmd.Parameters.AddWithValue("@id_fk", obj.entID_pk);
                                cmd.ExecuteNonQuery();
                            }
                            else if (nOpcion == 2) 
                            {
                                cmd.CommandText = @"UPDATE tblENTRENADORES SET entNOMBRE = @nombre, entAPELLIDO = @apellido 
                                                    WHERE entID_pk = @id";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@nombre", obj.entNOMBRE);
                                cmd.Parameters.AddWithValue("@apellido", obj.entAPELLIDO);
                                cmd.Parameters.AddWithValue("@id", obj.entID_pk);
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = @"SELECT COUNT(*) FROM tblTELEFONO_ENTRENADOR WHERE entID_fk = @id";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@id", obj.entID_pk);
                                int countTel = Convert.ToInt32(cmd.ExecuteScalar());

                                if (countTel > 0)
                                {
                                    cmd.CommandText = @"UPDATE tblTELEFONO_ENTRENADOR SET tel_entrenador = @telefono WHERE entID_fk = @id";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@telefono", obj.tel_entrenador);
                                    cmd.Parameters.AddWithValue("@id", obj.entID_pk);
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    cmd.CommandText = @"INSERT INTO tblTELEFONO_ENTRENADOR (tel_entrenadorID_pk, tel_entrenador, entID_fk) 
                                                       VALUES (@telId, @telefono, @id)";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@telId", GenerarNuevoIDTelefono(conn, trans));
                                    cmd.Parameters.AddWithValue("@telefono", obj.tel_entrenador);
                                    cmd.Parameters.AddWithValue("@id", obj.entID_pk);
                                    cmd.ExecuteNonQuery();
                                }

                                cmd.CommandText = @"SELECT COUNT(*) FROM tblCORREO_ENTRENADOR WHERE entID_fk = @id";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@id", obj.entID_pk);
                                int countCor = Convert.ToInt32(cmd.ExecuteScalar());

                                if (countCor > 0)
                                {
                                    cmd.CommandText = @"UPDATE tblCORREO_ENTRENADOR SET Cor_entrenador = @correo WHERE entID_fk = @id";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@correo", obj.cor_entrenador);
                                    cmd.Parameters.AddWithValue("@id", obj.entID_pk);
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    cmd.CommandText = @"INSERT INTO tblCORREO_ENTRENADOR (cor_entrenadorID_pk, Cor_entrenador, entID_fk) 
                                                       VALUES (@corId, @correo, @id)";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@corId", GenerarNuevoIDCorreo(conn, trans));
                                    cmd.Parameters.AddWithValue("@correo", obj.cor_entrenador);
                                    cmd.Parameters.AddWithValue("@id", obj.entID_pk);
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

        public string Eliminar_Entrenador(int entID_pk)
        {
            string respuesta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.geInstancia().CrearConexion())
                {
                    conn.Open();
                    using (MySqlTransaction trans = conn.BeginTransaction())
                    {
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = trans;

                        try
                        {
                            cmd.CommandText = "DELETE FROM tblTELEFONO_ENTRENADOR WHERE entID_fk = @id";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", entID_pk);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "DELETE FROM tblCORREO_ENTRENADOR WHERE entID_fk = @id";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", entID_pk);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "DELETE FROM tblENTRENADORES WHERE entID_pk = @id";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", entID_pk);
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
            MySqlCommand cmd = new MySqlCommand("SELECT IFNULL(MAX(tel_entrenadorID_pk), 0) + 1 FROM tblTELEFONO_ENTRENADOR", conn, trans);
            int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
            return nuevoId;
        }

        private int GenerarNuevoIDCorreo(MySqlConnection conn, MySqlTransaction trans)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT IFNULL(MAX(cor_entrenadorID_pk), 0) + 1 FROM tblCORREO_ENTRENADOR", conn, trans);
            int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
            return nuevoId;
        }
    }
}
