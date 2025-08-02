using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_Tel_Emp
    {
        public int pk_tel_empleado_id { get; set; }
        public int tel_numero { get; set; }
        public string fk_empleado_id { get; set; }
    }
}
