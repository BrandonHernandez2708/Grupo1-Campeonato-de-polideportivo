using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.DataAccess
{
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
                }

            }
            catch (Exception ex) 
            {
               throw new Exception("hay un error verificando el usuario:" + ex.Message, ex);
            }
            return tabla;

        }
    }
}
