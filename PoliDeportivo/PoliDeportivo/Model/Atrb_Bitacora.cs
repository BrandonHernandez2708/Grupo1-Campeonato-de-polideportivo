using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliDeportivo.Model
{
    internal class Atrb_Bitacora
    {
        public int pk_bitacora_id { get; set; }   
        public int fk_entidad_id { get; set; }   
        public char bit_operacion { get; set; }   // 'I' (Insert), 'U' (Update), 'D' (Delete)
        public DateTime bit_fecha_hora { get; set; } 
        public int fk_usuario_id { get; set; }   
        public string bit_ip { get; set; }      
    }
}
