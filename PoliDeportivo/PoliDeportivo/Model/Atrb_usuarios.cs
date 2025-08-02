using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    public class Atrb_usuario
    {
        public int pk_usuario_id { get; set; }
        public string usu_nombre { get; set; }
        public string usu_email { get; set; }
        public string usu_contrasena { get; set; }
        public int fk_privilegio_id { get; set; }
        public int fk_rol_id { get; set; }
        public DateTime usu_ultima_conexion { get; set; }
            
    }
}
