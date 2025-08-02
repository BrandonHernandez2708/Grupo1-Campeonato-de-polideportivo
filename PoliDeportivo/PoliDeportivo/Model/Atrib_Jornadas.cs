using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    public class Atrib_Jornadas
    {
        public int pk_jornada_id { get; set; }
        public int jor_cant_partidos { get; set; }
        public int fk_campeonato_id { get; set; }
    }
}