using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_Jugadores
    {
        public int pk_jugador_id { get; set; }
        public string jug_nombre { get; set; }
        public string jug_apellido { get; set; }
        public string jug_posicion { get; set; }
        public int fk_equipo_id { get; set; }
        
    }
}
