using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.DataAccess
{
    public class D_roles
    {
        public List<Atrb_roles> ObtenerRoles()

        {
            List<Atrb_roles> lista = new List<Atrb_roles>();

            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())

                {
                    string sql = "SELECT * FROM tbl_rol";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    
                    {
                        while (reader.Read())
                        {
                            Atrb_roles rol = new Atrb_roles()
                            {
                                pk_rol_id = Convert.ToInt32(reader["pk_rol_id"]),
                                rol_privilegio = reader["rol_privilegio"].ToString()
                            };
                            lista.Add(rol);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudieron obtener los roles" + ex.Message, ex);
            }

            return lista;
        }
    }
}


