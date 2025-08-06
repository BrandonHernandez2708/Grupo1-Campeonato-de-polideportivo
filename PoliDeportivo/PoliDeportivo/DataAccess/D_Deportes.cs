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
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "select \r\npk_deporte_id as \"Código Deporte\",\r\ndep_nombre as" +
                        " \"Nombre Deporte\",\r\ndep_cant_jugadores_equipo as \"Tamaño de Equipos\"," +
                        "\r\ndep_cant_jugadores_campo as \"Jugadores en el campo\"," +
                        "\r\ndep_cant_tiempos as \"Cantidad de tiempos\"," +
                        "\r\ndep_duracion_tiempo as \"Duración de Tiempos\"," +
                        "\r\ndep_duracion_total as \"Duración del partido\"" +
                        "\r\nfrom tbl_deporte;\r\n";
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
            string accion = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (nOpcion == 1) // Insertar
                    {
                        sql = @"INSERT INTO tbl_deporte (
                            pk_deporte_id,
                            dep_nombre,
                            dep_cant_jugadores_equipo,
                            dep_cant_jugadores_campo,
                            dep_cant_tiempos,
                            dep_duracion_tiempo,
                            dep_duracion_total
                        ) VALUES (
                            @id,
                            @nombre,
                            @cantEquipo,
                            @cantCampo,
                            @cantTiempos,
                            @durTiempo,
                            @durTotal
                        );";
                        accion = "I";
                    }
                    else // Actualizar
                    {
                        sql = @"UPDATE tbl_deporte SET
                            dep_nombre = @nombre,
                            dep_cant_jugadores_equipo = @cantEquipo,
                            dep_cant_jugadores_campo = @cantCampo,
                            dep_cant_tiempos = @cantTiempos,
                            dep_duracion_tiempo = @durTiempo,
                            dep_duracion_total = @durTotal
                        WHERE pk_deporte_id = @id;";
                        accion = "U";
                    }

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id", obj.depID_pk);
                    cmd.Parameters.AddWithValue("@nombre", obj.depnombre);
                    cmd.Parameters.AddWithValue("@cantEquipo", obj.depcantidad_jugadores_equipo);
                    cmd.Parameters.AddWithValue("@cantCampo", obj.depcantidad_jugadores_campo);
                    cmd.Parameters.AddWithValue("@cantTiempos", obj.depcantidad_de_tiemposdep);
                    cmd.Parameters.AddWithValue("@durTiempo", obj.depduracion_de_cada_tiempo);
                    cmd.Parameters.AddWithValue("@durTotal", obj.depduracion_total_del_partido);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows >= 1)
                    {
                        // Insertar en bitácora
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", obj.depID_pk);
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

        public string Eliminar_Dep(int depID_pk)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    conn.Open();
                    string sql = "DELETE FROM tbl_deporte WHERE pk_deporte_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", depID_pk);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows >= 1)
                    {
                        // Insertar en bitácora
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

                        cmdBitacora.Parameters.AddWithValue("@entidad", 3);
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