using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alquinet_Entidad
{
    //    CREATE TABLE[Agente] (
    //  [cod PK] integer,
    //  [telefono] varchar,
    //  [nombre] varchar,
    //  [apellido] varchar,
    //  [ci UNIQUE] varchar,
    //  [correo UNIQUE] varchar,
    //  [contraseña] varchar
    //);
    public class Agente : Persona
    {
        public string Horario { get; set; }
        public List<Propiedad> Propiedades { get; set; }
    }
}
