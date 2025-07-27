
using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Data;

namespace PoliDeportivo.DataAccess
{
    public class D_Deportes
    {
        public DataTable Listado_Dep()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.geInstancia().CrearConexion())
                {
                    string sql = "SELECT * FROM tbldeporte";
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

        public string Guardar_Dep(int nOpcion, Atrb_deportes obj)
        {
            string Rpta = "";
            string sql = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.geInstancia().CrearConexion())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (nOpcion == 1) // Insertar
                    {
                        sql = @"INSERT INTO tbldeporte(depID_pk, DEPNOMBRE, DEPCANTIDAD_JUGADORES_EQUIPO,
                                DEPCANTIDAD_JUGADORES_EN_EL_CAMPO, DEPCANTIDAD_DE_TIEMPOSDEP, DEPDURACION_DE_CADA_TIEMPO,
                                DEPDURACION_TOTAL_DEL_PARTIDO)
                                VALUES (@id, @nombre, @cantEquipo, @cantCampo, @cantTiempos, @durTiempo, @durTotal)";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", obj.depID_pk);
                        cmd.Parameters.AddWithValue("@nombre", obj.depnombre);
                        cmd.Parameters.AddWithValue("@cantEquipo", obj.depcantidad_jugadores_equipo);
                        cmd.Parameters.AddWithValue("@cantCampo", obj.depcantidad_jugadores_campo);
                        cmd.Parameters.AddWithValue("@cantTiempos", obj.depcantidad_de_tiemposdep);
                        cmd.Parameters.AddWithValue("@durTiempo", obj.depduracion_de_cada_tiempo);
                        cmd.Parameters.AddWithValue("@durTotal", obj.depduracion_total_del_partido);
                    }
                    else // Actualizar
                    {
                        sql = @"UPDATE tbldeporte SET 
                                DEPNOMBRE = @nombre,
                                DEPCANTIDAD_JUGADORES_EQUIPO = @cantEquipo,
                                DEPCANTIDAD_JUGADORES_EN_EL_CAMPO = @cantCampo,
                                DEPCANTIDAD_DE_TIEMPOSDEP = @cantTiempos,
                                DEPDURACION_DE_CADA_TIEMPO = @durTiempo,
                                DEPDURACION_TOTAL_DEL_PARTIDO = @durTotal
                                WHERE depID_pk = @id";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", obj.depID_pk);
                        cmd.Parameters.AddWithValue("@nombre", obj.depnombre);
                        cmd.Parameters.AddWithValue("@cantEquipo", obj.depcantidad_jugadores_equipo);
                        cmd.Parameters.AddWithValue("@cantCampo", obj.depcantidad_jugadores_campo);
                        cmd.Parameters.AddWithValue("@cantTiempos", obj.depcantidad_de_tiemposdep);
                        cmd.Parameters.AddWithValue("@durTiempo", obj.depduracion_de_cada_tiempo);
                        cmd.Parameters.AddWithValue("@durTotal", obj.depduracion_total_del_partido);
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

        public string Eliminar_Dep(int depID_pk)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.geInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbldeporte WHERE depID_pk = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", depID_pk);
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
