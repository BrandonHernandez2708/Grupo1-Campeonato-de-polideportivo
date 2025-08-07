using MySql.Data.MySqlClient;
using PoliDeportivo;
using PoliDeportivo.Model;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PoliDeportivo.DataAccess
{
    class D_Partidos
    {

        public DataTable Listado_Partidos()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"SELECT
                                p.pk_partido_id AS 'Código Partido',
                                p.fk_jornada_id AS 'ID Jornada',
    
                                p.fk_equipo1_id AS 'ID Equipo 1',
                                eq1.equ_nombre AS 'Equipo 1',
    
                                p.fk_equipo2_id AS 'ID Equipo 2',
                                eq2.equ_nombre AS 'Equipo 2',

                                c.pk_cancha_id AS 'Código Cancha',
    
                                p.fk_empleado_arbitro_id AS 'Árbitro ID',
                                CONCAT(emp.emp_nombre, ' ', emp.emp_apellido) AS 'Árbitro',
    
                                p.par_fecha_hora AS 'Fecha y Hora',
                                p.par_puntaje_equipo1 AS 'Puntaje Equipo 1',
                                p.par_puntaje_equipo2 AS 'Puntaje Equipo 2',
                                p.par_tiempo_extra AS 'Tiempo Extra',

                                p.fk_equipo_ganador_id AS 'ID Equipo Ganador',
                                eqg.equ_nombre AS 'Equipo Ganador',

                                p.fk_estado_id AS 'ID Estado',
                                est.est_descripcion AS 'Estado del Partido'
                            FROM tbl_partido p
                            INNER JOIN tbl_jornada j ON p.fk_jornada_id = j.pk_jornada_id
                            INNER JOIN tbl_equipo eq1 ON p.fk_equipo1_id = eq1.pk_equipo_id
                            INNER JOIN tbl_equipo eq2 ON p.fk_equipo2_id = eq2.pk_equipo_id
                            INNER JOIN tbl_cancha c ON p.fk_cancha_id = c.pk_cancha_id
                            INNER JOIN tbl_empleado emp ON p.fk_empleado_arbitro_id = emp.pk_empleado_id
                            LEFT JOIN tbl_equipo eqg ON p.fk_equipo_ganador_id = eqg.pk_equipo_id
                            LEFT JOIN tbl_estado_partido est ON p.fk_estado_id = est.pk_estado_partido_id; ";

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
                throw new Exception("Error al obtener los partidos: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Partido(int nOpcion, Atrb_Partidos obj)
        {
            string Rpta = "";
            string sql;
            string accion = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (nOpcion == 1) // INSERT
                    {
                        accion = "I";
                        sql = @"INSERT INTO tbl_partido 
                        (pk_partido_id, fk_jornada_id, fk_equipo1_id, fk_equipo2_id, fk_cancha_id, fk_empleado_arbitro_id,
                         par_fecha_hora, par_puntaje_equipo1, par_puntaje_equipo2, fk_equipo_ganador_id, 
                         fk_estado_id, par_tiempo_extra)
                        VALUES
                        (@id, @jornada, @equipo1, @equipo2, @cancha, @arbitro, @fecha, @puntaje1, @puntaje2, @ganador, @estado, @tiempoExtra);";
                    }
                    else // UPDATE
                    {
                        accion = "U";
                        sql = @"UPDATE tbl_partido SET 
                        fk_jornada_id = @jornada,
                        fk_equipo1_id = @equipo1,
                        fk_equipo2_id = @equipo2,
                        fk_cancha_id = @cancha,
                        fk_empleado_arbitro_id = @arbitro,
                        par_fecha_hora = @fecha,
                        par_puntaje_equipo1 = @puntaje1,
                        par_puntaje_equipo2 = @puntaje2,
                        fk_equipo_ganador_id = @ganador,
                        fk_estado_id = @estado,
                        par_tiempo_extra = @tiempoExtra
                    WHERE pk_partido_id = @id;";
                    }

                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@id", obj.pk_partido_id);
                    cmd.Parameters.AddWithValue("@jornada", obj.fk_jornada_id); // <- Agregado
                    cmd.Parameters.AddWithValue("@equipo1", obj.fk_equipo1_id);
                    cmd.Parameters.AddWithValue("@equipo2", obj.fk_equipo2_id);
                    cmd.Parameters.AddWithValue("@cancha", obj.fk_cancha_id);
                    cmd.Parameters.AddWithValue("@arbitro", obj.fk_empleado_arbitro_id);
                    cmd.Parameters.AddWithValue("@fecha", obj.par_fecha_hora);
                    cmd.Parameters.AddWithValue("@puntaje1", obj.par_puntaje_equipo1);
                    cmd.Parameters.AddWithValue("@puntaje2", obj.par_puntaje_equipo2);
                    cmd.Parameters.AddWithValue("@ganador", obj.fk_equipo_ganador_id);
                    cmd.Parameters.AddWithValue("@estado", obj.fk_estado_id);
                    cmd.Parameters.AddWithValue("@tiempoExtra", obj.par_tiempo_extra);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows >= 1)
                    {
                        // Bitácora
                        MySqlCommand cmdBitacora = new MySqlCommand();
                        cmdBitacora.Connection = conn;
                        cmdBitacora.CommandText = @"INSERT INTO tbl_bitacora (
                                                        fk_entidad_id,
                                                        bit_operacion,
                                                        bit_fecha_hora,
                                                        fk_usuario_id,
                                                        bit_ip
                                                    ) VALUES (
                                                        @entidad,
                                                        @operacion,
                                                        NOW(),
                                                        @usuarioId,
                                                        @ip
                                                    );";

                        cmdBitacora.Parameters.AddWithValue("@entidad", 20);
                        cmdBitacora.Parameters.AddWithValue("@operacion", accion);
                        cmdBitacora.Parameters.AddWithValue("@usuarioId", Sesion.UsuarioId);
                        cmdBitacora.Parameters.AddWithValue("@ip", ObtenerIPLocal());

                        cmdBitacora.ExecuteNonQuery();

                        Rpta = "OK";
                    }
                    else
                    {
                        Rpta = "No se pudo guardar";
                    }
                }
            }
            catch (Exception ex)
            {
                Rpta = ex.Message;
            }
            return Rpta;
        }

        public string Eliminar_Partido(int pk_partido_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_partido WHERE pk_partido_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_partido_id);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows >= 1)
                    {
                        // Bitácora
                        MySqlCommand cmdBitacora = new MySqlCommand();
                        cmdBitacora.Connection = conn;
                        cmdBitacora.CommandText = @"INSERT INTO tbl_bitacora (
                                                        fk_entidad_id,
                                                        bit_operacion,
                                                        bit_fecha_hora,
                                                        fk_usuario_id,
                                                        bit_ip
                                                    ) VALUES (
                                                        @entidad,
                                                        @operacion,
                                                        NOW(),
                                                        @usuarioId,
                                                        @ip
                                                    );";

                        cmdBitacora.Parameters.AddWithValue("@entidad", 20);
                        cmdBitacora.Parameters.AddWithValue("@operacion", "D");
                        cmdBitacora.Parameters.AddWithValue("@usuarioId", Sesion.UsuarioId);
                        cmdBitacora.Parameters.AddWithValue("@ip", ObtenerIPLocal());

                        cmdBitacora.ExecuteNonQuery();
                        Rpta = "OK";
                    }
                    else
                    {
                        Rpta = "No se pudo eliminar";
                    }
                }
            }
            catch (Exception ex)
            {
                Rpta = ex.Message;
            }
            return Rpta;
        }

        private string ObtenerIPLocal()
        {
            string ip = "";
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ipAddr in host.AddressList)
                {
                    if (ipAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ip = ipAddr.ToString();
                        break;
                    }
                }
            }
            catch
            {
                ip = "No disponible";
            }
            return ip;
        }

    }
}