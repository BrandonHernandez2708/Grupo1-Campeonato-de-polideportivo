using MySql.Data.MySqlClient;
using PoliDeportivo.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.DataAccess
{
public class D_privilegios
    {
        public List<Atrb_privilegios> ListarPrivilegios()
        {
            List<Atrb_privilegios> lista = new List<Atrb_privilegios>();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "SELECT * FROM tbl_privilegio";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        lista.Add(new Atrb_privilegios
                        {
                            pk_privilegio_id = Convert.ToInt32(reader["pk_privilegio_id"]),
                            priv_descripcion = reader["priv_descripcion"].ToString()
                        });
                    }
                }
            }
            }
            catch (Exception ex) 
            {
                throw new Exception("no se pueden listar los privilegios" + ex.Message);
            }
            return lista;
        }
    }
}







