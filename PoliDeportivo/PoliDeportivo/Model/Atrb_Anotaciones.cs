using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_Anotaciones
    {
        public int pk_anotacion_id { get; set; }
        public int ano_minuto { get; set; }
        public string ano_descripcion { get; set; }
        public int fk_partido_id { get; set; }
        public int fk_jugador_id { get; set; }

    }
}
