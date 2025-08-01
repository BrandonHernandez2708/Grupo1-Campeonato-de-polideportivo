using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    public class Atrb_campeonatos
    {
        public int pk_campeonato_id { get; set; }
        public string cam_nombre { get; set; }
        public string cam_modalidad { get; set; }
        public int cam_cant_equipos { get; set; }
        public DateTime cam_fecha_inicio { get; set; }
        public DateTime cam_fecha_final { get; set; }
        public int jor_cant_partidos { get; set; }
    }
}