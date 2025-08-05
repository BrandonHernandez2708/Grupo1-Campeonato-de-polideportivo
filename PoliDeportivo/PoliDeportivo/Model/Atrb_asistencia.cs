using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_asistencia
    {
        public int pk_asistencia_id { get; set; }      
        public int fk_anotacion_id { get; set; }       
        public int fk_jugador_id { get; set; }
    }
}
