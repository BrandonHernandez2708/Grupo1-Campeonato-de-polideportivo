using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_Empleados
    {
        public long pk_empleado_id { get; set; }
        public string emp_nombre { get; set; }
        public string emp_apellido { get; set; }



        public DateTime con_fecha { get; set; }
        public byte con_tipo_operacion { get; set; }
    }
}
