using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alquinet_Entidad
{
    //    CREATE TABLE `Alquilar` (
    //  `cod PK` integer,
    //  `fecha` date,
    //  `cod_usuario FK` integer,
    //  `cod_agente FK` integer,
    //  `cod_propiedad FK` integer
    //);
    public class Visita
    {
        public int Cod { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Estado { get; set; }
        public int Cod_usuario { get; set; }
        public int Cod_propiedad { get; set; }
        public int Cod_agente { get; set; }
        
        public Usuario Usuario { get; set; }
        public Propiedad Propiedad { get; set; }
        public Agente Agente { get; set; }
    }
}
