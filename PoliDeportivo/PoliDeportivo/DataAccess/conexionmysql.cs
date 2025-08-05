using MySql.Data.MySqlClient;
using System;

namespace PoliDeportivo
{
    public class conexionmysql
    {
        private string BD;
        private string servidor;
        private string puerto;
        private string usuario;
        private string clave;
        private static conexionmysql Conexion = null;

        private conexionmysql()
        {
            this.BD = "bdPolideportivo";   // Cambia al nombre de tu BD
            this.servidor = "172.20.10.5"; // IP del servidor MySQL
            this.puerto = "3306";
            this.usuario = "Grupo1";       // Tu usuario MySQL
            this.clave = "Grupo1";         // Tu contraseña MySQL
        }

        public MySqlConnection CrearConexion()
        {
            MySqlConnection conexion = new MySqlConnection();
            try
            {
                conexion.ConnectionString = "Server=" + this.servidor +
                                            ";Port=" + this.puerto +
                                            ";User Id=" + this.usuario +
                                            ";Password=" + this.clave +
                                            ";Database=" + this.BD +
                                            ";SslMode=None;" +
                                            "AllowPublicKeyRetrieval=True;";
            }
            catch (Exception ex)
            {
                conexion = null;
                throw new Exception("Error en la conexión: " + ex.Message, ex);
            }
            return conexion;
        }

        public static conexionmysql getInstancia()
        {
            if (Conexion == null)
            {
                Conexion = new conexionmysql();
            }
            return Conexion;
        }
    }
}
