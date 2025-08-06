using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Data;

namespace PoliDeportivo.DataAccess
{
    class D_Jugadores
    {
        public DataTable Listado_Jugadores()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"SELECT
                                pk_jugador_id AS 'Código Jugador',
                                jug_nombre AS 'Nombre del Jugador',
                                jug_apellido AS 'Apellido del Jugador',
                                jug_posicion AS 'Posición',
                                pk_equipo_id AS 'ID Equipo',
                                equ_nombre AS 'Nombre del Equipo',
                                dep_nombre AS 'Nombre del Deporte'
                                FROM tbl_jugador j
                                INNER JOIN tbl_equipo e ON j.fk_equipo_id = e.pk_equipo_id
                                INNER JOIN tbl_deporte d ON e.fk_deporte_id = d.pk_deporte_id;";
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
                throw new Exception("Error al obtener jugador: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Jugador(int nOpcion, Atrb_Jugadores obj)
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
                        sql = @"INSERT INTO tbl_jugador 
                                (pk_jugador_id, jug_nombre, jug_apellido, jug_posicion, fk_equipo_id) 
                                VALUES 
                                (@id_jugador, @nombre, @apellido, @posicion, @fk_equipo_id);";
                    }
                    else // UPDATE
                    {

                        accion = "U";
                        sql = @"UPDATE tbl_jugador SET 
                                jug_nombre = @nombre, 
                                jug_apellido = @apellido,
                                jug_posicion = @posicion,
                                fk_equipo_id = @fk_equipo_id
                                WHERE pk_jugador_id = @id_jugador;";
                    }

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id_jugador", obj.pk_jugador_id);
                    cmd.Parameters.AddWithValue("@nombre", obj.jug_nombre);
                    cmd.Parameters.AddWithValue("@apellido", obj.jug_apellido);
                    cmd.Parameters.AddWithValue("@posicion", obj.jug_posicion); // ← Esto faltaba
                    cmd.Parameters.AddWithValue("@fk_equipo_id", obj.fk_equipo_id);

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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 17);
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

        public string Eliminar_Jugador(int pk_jugador_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_jugador WHERE pk_jugador_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_jugador_id);
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 17);
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
