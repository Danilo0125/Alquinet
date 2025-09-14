using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alquinet_Entidad
{
    public abstract class Persona
    {
        public int Cod { get; set; }
        public string Telefono { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Ci { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
    }
}
