using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Data;

namespace PoliDeportivo.DataAccess
{
    class D_equipos
    {
        public DataTable Listado_Equipo()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"SELECT
                                    pk_equipo_id AS 'Código Equipo',
                                    equ_nombre AS 'Nombre del Equipo',
                                    equ_cant_integrantes AS 'Cantidad integrantes',
                                    equ_telefono AS 'Teléfono del Equipo',
                                    equ_correo AS 'Correo del Equipo',
                                    d.pk_deporte_id AS 'Código Deporte',
                                    d.dep_nombre AS 'nombre_deporte',
                                    t.pk_entrenador_id AS 'Código Entrenador',
                                    t.ent_nombre AS 'nombre_entrenador',
                                    t.ent_apellido AS 'apellido_entrenador'
                                FROM tbl_equipo e
                                INNER JOIN tbl_deporte d ON e.fk_deporte_id = d.pk_deporte_id
                                INNER JOIN tbl_entrenador t ON e.fk_entrenador_id = t.pk_entrenador_id;";
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
                throw new Exception("Error al obtener equipos: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Equipo(int nOpcion, Atrb_equipo obj)
        {
            string Rpta = "";
            string sql;

            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (nOpcion == 1) // INSERT
                    {
                        sql = @"INSERT INTO tbl_equipo 
                                (pk_equipo_id, equ_nombre, fk_deporte_id, equ_cant_integrantes, fk_entrenador_id, equ_telefono, equ_correo) 
                                VALUES 
                                (@id_equipo, @nombre, @fk_deporte_id, @cantIntegrantes, @fk_entrenador_id, @telefono, @correo);";
                    }
                    else // UPDATE
                    {
                        sql = @"UPDATE tbl_equipo SET 
                                equ_nombre = @nombre, 
                                fk_deporte_id = @fk_deporte_id,
                                equ_cant_integrantes = @cantIntegrantes,
                                fk_entrenador_id = @fk_entrenador_id,
                                equ_telefono = @telefono,
                                equ_correo = @correo
                            WHERE pk_equipo_id = @id_equipo;";
                    }

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id_equipo", obj.pk_equipo_id);
                    cmd.Parameters.AddWithValue("@nombre", obj.equipo_nombre);
                    cmd.Parameters.AddWithValue("@fk_deporte_id", obj.fk_deporte_id);
                    cmd.Parameters.AddWithValue("@cantIntegrantes", obj.cant_integrantes);
                    cmd.Parameters.AddWithValue("@fk_entrenador_id", obj.fk_entrenador_id);
                    cmd.Parameters.AddWithValue("@telefono", obj.equipo_telefono);
                    cmd.Parameters.AddWithValue("@correo", obj.equipo_correo);

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

        public string Eliminar_Equipo(int pk_equipo_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_equipo WHERE pk_equipo_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_equipo_id);
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
