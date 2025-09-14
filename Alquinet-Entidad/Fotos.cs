using System.Data.SqlTypes;
using System.Reflection;
using System.Security;

namespace Alquinet_Entidad
{
    public class Fotos
    {
        public int Cod { get; set; }
        public string Ambiente { get; set; }
        public string DireccionFoto { get; set; }
        public string Base64 { get; set; }
        public int Cod_propiedad { get; set; }
        //public string Extencion {  get; set; }
    }
}