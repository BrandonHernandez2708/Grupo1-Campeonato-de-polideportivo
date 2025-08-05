using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_sancion
    {
        public int pk_sancion_id { get; set; }         // Clave primaria autoincremental
        public string san_descripcion { get; set; }    // Descripción de la sanción
        public int san_minuto { get; set; }            // Minuto en que ocurrió la sanción
        public int fk_partido_id { get; set; }         // Clave foránea a partido
        public int fk_jugador_id { get; set; }
    }
}
