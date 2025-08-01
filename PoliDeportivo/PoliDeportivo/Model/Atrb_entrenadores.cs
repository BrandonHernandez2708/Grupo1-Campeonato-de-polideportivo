using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    public class Atrb_entrenadores
    {
        public int pk_entrenador_id { get; set; }
        public string ent_nombre { get; set; }
        public string ent_apellido { get; set; }
        public string tel_numero { get; set; }
        public string correo { get; set; }
    }
}
