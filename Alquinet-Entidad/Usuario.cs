using System.Collections.Generic;

namespace Alquinet_Entidad
{
    public class Usuario: Persona
    {
        public Usuario(string correo, string clave)
        {
            Correo = correo;
            Contraseña = clave;
        }
        public Usuario() { }
        public List<Propiedad> Propiedades { get; set; }
    }
}