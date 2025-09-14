using Alquinet_Datos;
using Alquinet_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Alquinet_Negocio
{
    public class VisitaService
    {
        private readonly VisitaDatos _vista = new VisitaDatos();
        public List<Visita> Listar(int cod, string estado)
        {
            return _vista.Listar(cod, estado);
        }
        public Visita GetById(int id)
        {
            return _vista.GetById(id);  
        }
        public int Registrar(Visita visita, out string mensaje)
        {
            return _vista.Registrar(visita, out mensaje);
        }
        public bool Editar(Visita visita, out string mensaje)
        {
            return _vista.Editar(visita, out mensaje);
        }
        public bool Eliminar(int cod, out string mensaje)
        {
            return _vista.Eliminar(cod, out mensaje);
        }
    }
}
