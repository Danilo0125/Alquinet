using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Alquinet_Entidad.ClasesPropiedad
{
    public class Casa : Propiedad
    {
        public string Pisos {  get; set; }
    }

    public class Departamento : Propiedad
    {
        public string NombreEdificio { get; set; }
        public string Planta { get; set; }
    }
}
