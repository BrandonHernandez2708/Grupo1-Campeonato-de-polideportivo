using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    public class Atrb_deportes
    {
        public int depID_pk { get; set; }
        public string depnombre { get; set; }
        public int depcantidad_jugadores_equipo { get; set; }
        public int depcantidad_jugadores_campo { get; set; }
        public int depcantidad_de_tiemposdep { get; set; }
        public int depduracion_de_cada_tiempo { get; set; }
        public int depduracion_total_del_partido { get; set; }
    }
}
