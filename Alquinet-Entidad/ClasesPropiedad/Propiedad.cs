using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alquinet_Entidad
{
    //    CREATE TABLE[Propiedad] (
    //  [cod PK] integer,
    //  [disponibilidad] boolean,
    //  [tipo] varchar,
    //  [precio] float,
    //  [descripcion] text,
    //  [cod_usuario FK] integer
    //);
    public class Propiedad
    {
        public int Cod { get; set; }
        public string Titulo { get; set; }
        public decimal Area { get; set; }
        public string Tipo { get; set; }
        public string Disponibilidad { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string PrecioTexto {  get; set; }
        public int Cod_usuario { get; set; }
        public int Cod_agente { get; set; }
        public int Cod_direccion { get; set; }
        public Direccion Direccion { get; set; }
        public Usuario Usuario { get; set; }
        public Agente Agente { get; set; }
        public List<Fotos> Fotos { get; set; }
    }
}
