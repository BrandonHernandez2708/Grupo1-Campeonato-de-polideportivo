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
    internal class D_Login
    {
        public DataTable Listado_Login()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"SELECT
                        pk_usuario_id AS 'Código Login',
                        usu_nombre AS 'Nombre del usuario',
                        usu_email AS 'Email',
                        usu_contrasena AS 'Contraseña',
                        fk_privilegio_id AS 'Privilegios',
                        fk_rol_id AS 'Roles'
                        FROM tbl_usuario
                        ORDER BY pk_usuario_id;";
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
                throw new Exception("Error al obtener Login: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Login(int nOpcion, Atrb_Login obj)
        {
            string Rpta = "";
            string sql = "";

            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (nOpcion == 1) // INSERT
                    {
                        sql = @" INSERT INTO tbl_usuario (
                                pk_usuario_id,
                                usu_nombre,
                                usu_email,
                                usu_contrasena,
                                fk_privilegio_id,
                                fk_rol_id
                                
                            ) VALUES (
                                @id,
                                @nombre,
                                @email,
                                @contrasena,
                                @privilegioID,
                                @rolID
                               
                            );";
                        cmd.CommandText = sql;

                        // Parámetros comunes
                        cmd.Parameters.AddWithValue("@id_usuario", obj.pk_usuario_id);
                        cmd.Parameters.AddWithValue("@nombre", obj.usuario);
                        cmd.Parameters.AddWithValue("@email", obj.user_email);
                        cmd.Parameters.AddWithValue("@contraseña", obj.user_contraseña);
                        cmd.Parameters.AddWithValue("@privilegios", obj.fk_id_privilegios);
                        cmd.Parameters.AddWithValue("@roles", obj.fk_id_roles);
                        
                    }
                    else // UPDATE
                    {
                        sql = @"UPDATE tbl_usuario SET
                                usu_nombre = @nombre,
                                usu_email = @email,
                                usu_contrasena = @contrasena,
                                fk_privilegio_id = @privilegioID,
                                fk_rol_id = @rolID,
                               
                            WHERE pk_usuario_id = @id;";

                        cmd.CommandText = sql;

                        // Parámetros comunes
                        cmd.Parameters.AddWithValue("@id_usuario", obj.pk_usuario_id);
                        cmd.Parameters.AddWithValue("@nombre", obj.usuario);
                        cmd.Parameters.AddWithValue("@email", obj.user_email);
                        cmd.Parameters.AddWithValue("@contraseña", obj.user_contraseña);
                        cmd.Parameters.AddWithValue("@privilegios", obj.fk_id_privilegios);
                        cmd.Parameters.AddWithValue("@roles", obj.fk_id_roles);
                        
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

        public string Eliminar_Login(int pk_usuario_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = @"DELETE FROM tbl_usuario WHERE pk_usuario_id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", pk_usuario_id);
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
