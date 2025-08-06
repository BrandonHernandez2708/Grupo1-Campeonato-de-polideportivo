using MySql.Data.MySqlClient;
using PoliDeportivo;
using PoliDeportivo.Model;
using PoliDeportivo.Utils;
using System.Data;
using System.Reflection.PortableExecutable;

public class D_usuarios
{
    public DataTable Login(string usuario, string contrasena)
    {
        DataTable tabla = new DataTable();
        try
        {
            using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
            {
                string sql = @"SELECT u.*, r.rol_privilegio, p.priv_descripcion
                           FROM tbl_usuario u
                           INNER JOIN tbl_rol r ON u.fk_rol_id = r.pk_rol_id
                           INNER JOIN tbl_privilegio p ON u.fk_privilegio_id = p.pk_privilegio_id
                           WHERE u.usu_nombre = @usuario AND u.usu_contrasena = @contrasena";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);

                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    tabla.Load(reader);
                }

                //Verifica si el usuario existe y actualiza la última conexión
                if (tabla.Rows.Count > 0)
                {

                    string updateSql = @"UPDATE tbl_usuario
                                     SET usu_ultima_conexion = NOW()
                                     WHERE usu_nombre = @usuario AND usu_contrasena = @contrasena";

                    MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@usuario", usuario);
                    updateCmd.Parameters.AddWithValue("@contrasena", contrasena);
                    updateCmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Hay un error verificando el usuario: " + ex.Message, ex);
        }
        return tabla;
    }
        //metodos para el formulario
        public DataTable Listado_usuario()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "SELECT \r\n" +
                                 "    pk_usuario_id AS \"Código Usuario\",\r\n" +
                                 "    usu_nombre AS \"Nombre de Usuario\",\r\n" +
                                 "    usu_email AS \"Correo Electrónico\",\r\n" +
                                 "    usu_contrasena AS \"Contraseña\",\r\n" +
                                 "    fk_privilegio_id AS \"Código Privilegio\",\r\n" +
                                 "    fk_rol_id AS \"Código Rol\",\r\n" +
                                 "    usu_ultima_conexion AS \"Última Conexión\"\r\n" +
                                 "FROM tbl_usuario;\r\n";
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
                throw new Exception("Error al obtener Usuarios: " + ex.Message, ex);
            }
            return Tabla;
        }

        public string Guardar_Usuario(int nOpcion, Atrb_usuario obj)
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
                        sql = @"INSERT INTO tbl_usuario (
                                        pk_usuario_id,
                                        usu_nombre,
                                        usu_email,
                                        usu_contrasena,
                                        fk_privilegio_id,
                                        fk_rol_id
                                    ) VALUES (
                                        @codigo,
                                        @nombre,
                                        @correo,
                                        @contrasena,
                                        @privilegioId,
                                        @rolId
                                    );
                                    ";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@codigo", obj.pk_usuario_id);
                        cmd.Parameters.AddWithValue("@nombre", obj.usu_nombre);
                        cmd.Parameters.AddWithValue("@correo", obj.usu_email);
                        string contrasenaHasheada = Proteccion.HashPassword(obj.usu_contrasena);
                        cmd.Parameters.AddWithValue("@contrasena", contrasenaHasheada);
                        cmd.Parameters.AddWithValue("@privilegioId", obj.fk_privilegio_id);
                        cmd.Parameters.AddWithValue("@rolId", obj.fk_rol_id);
             
                    }
                    else // Actualizar
                    {
                        sql = @"UPDATE tbl_usuario SET       
                                    usu_nombre = @nombre,
                                    usu_email = @correo,
                                    usu_contrasena = @contrasena,
                                    fk_privilegio_id = @privilegioId,
                                    fk_rol_id = @rolId
                                WHERE pk_usuario_id = @codigo;";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@codigo", obj.pk_usuario_id);
                        cmd.Parameters.AddWithValue("@nombre", obj.usu_nombre);
                        cmd.Parameters.AddWithValue("@correo", obj.usu_email);
                        string contrasenaHasheada = Proteccion.HashPassword(obj.usu_contrasena);
                        cmd.Parameters.AddWithValue("@contrasena", contrasenaHasheada);
                        cmd.Parameters.AddWithValue("@privilegioId", obj.fk_privilegio_id);
                        cmd.Parameters.AddWithValue("@rolId", obj.fk_rol_id);
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

        public string Eliminar_Usuario(int pk_usuario_id)
        {
            string Rpta = "";
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "DELETE FROM tbl_usuario\r\n" +
                        "WHERE pk_usuario_id = @codigo";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@codigo", pk_usuario_id);
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

