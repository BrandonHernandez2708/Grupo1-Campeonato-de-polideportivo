using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace PoliDeportivo.DataAccess
{
    internal class D_Bitacora
    {
        public DataTable Listado_Bitacora()
        {
            DataTable Tabla = new DataTable();
            try
            {
                using (MySqlConnection conn = conexionmysql.getInstancia().CrearConexion())
                {
                    string sql = "SELECT \r\n" +
                          "b.pk_bitacora_id AS \"Código Bitácora\",\r\n" +
                          "e.ent_nombre AS \"Entidad\",\r\n" +
                          "u.usu_nombre AS \"Usuario\",\r\n" +
                          "CASE b.bit_operacion \r\n" +
                          "    WHEN 'I' THEN 'INSERT'\r\n" +
                          "    WHEN 'U' THEN 'UPDATE'\r\n" +
                          "    WHEN 'D' THEN 'DELETE'\r\n" +
                          "END AS \"Operación\",\r\n" +
                          "b.bit_fecha_hora AS \"Fecha y Hora\",\r\n" +
                          "b.bit_ip AS \"Dirección IP\"\r\n" +
                          "FROM tbl_bitacora b\r\n" +
                          "INNER JOIN tbl_entidad e ON b.fk_entidad_id = e.pk_entidad_id\r\n" +
                          "INNER JOIN tbl_usuario u ON b.fk_usuario_id = u.pk_usuario_id\r\n" +
                          "ORDER BY b.bit_fecha_hora DESC;";
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
                throw new Exception("Error al obtener Bitacora: " + ex.Message, ex);
            }
            return Tabla;
        }
    }
}
