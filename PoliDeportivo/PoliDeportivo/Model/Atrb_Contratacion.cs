using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_Contratacion
    {
        public int fk_puesto_id { get; set; }
        public int fk_empleado_id { get; set; }
        public DateTime con_fecha { get; set; }
        public int con_tipo_operacion { get; set; } 
    }
}
