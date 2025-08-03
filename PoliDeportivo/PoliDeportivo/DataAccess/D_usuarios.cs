using MySql.Data.MySqlClient;
using PoliDeportivo;
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
}