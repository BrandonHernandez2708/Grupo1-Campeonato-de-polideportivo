using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_equipo
    {
        public int pk_equipo_id { get; set; }
        public string equipo_nombre { get; set; }
        public int fk_deporte_id { get; set; }
        public int cant_integrantes { get; set; }
        public int fk_entrenador_id { get; set; }
        public string equipo_telefono { get; set; }
        public string equipo_correo { get; set; }
    }
}
