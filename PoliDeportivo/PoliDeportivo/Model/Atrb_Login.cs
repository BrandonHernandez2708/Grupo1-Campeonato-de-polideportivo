using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_Login
    {
        public int pk_usuario_id { get; set; }
        public string usuario { get; set; }
        public string user_email { get; set; }
        public string user_contraseña { get; set; }
        public int fk_id_privilegios { get; set; }
        public int fk_id_roles { get; set; }



    }
}
