using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_Falta
    {
        public int pk_falta_id { get; set; }
        public string fal_descripcion { get; set; }
        public int fal_minuto { get; set; }
        public int fk_partido_id { get; set; }
        public int fk_jugador_id { get; set; }
        

    }
}
