using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_Partidos
    {
      public int pk_partido_id { get; set; }
        public int fk_jornada_id { get; set; }
        public int fk_equipo1_id { get; set; }
        public int fk_equipo2_id { get; set; }
        public int fk_cancha_id { get; set; }
        public DateTime par_fecha_hora { get; set; }

        public int fk_estado_id { get; set; }
        public int fk_empleado_arbitro_id { get; set; }

        public int par_puntaje_equipo1 { get; set; }
        public int par_puntaje_equipo2 { get; set; }
        public bool par_tiempo_extra { get; set; }
        public int fk_equipo_ganador_id{ get; set; }
    }
}
