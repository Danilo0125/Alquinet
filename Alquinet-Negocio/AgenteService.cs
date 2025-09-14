using Alquinet_Datos;
using Alquinet_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alquinet_Negocio
{
    public class AgenteService
    {
        private AgenteDatos objDatos = new AgenteDatos();
        public List<Agente> Listar()
        {
            return objDatos.Listar();
        }
        public Agente UsuarioById(int id)
        {
            return objDatos.GetById(id);
        }
        public int Registrar(Agente agente, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(agente.Nombre) || string.IsNullOrWhiteSpace(agente.Nombre))
            {
                mensaje = "El nombre del agente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(agente.Apellido) || string.IsNullOrWhiteSpace(agente.Apellido))
            {
                mensaje = "El apellido del agente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(agente.Correo) || string.IsNullOrWhiteSpace(agente.Correo))
            {
                mensaje = "El Correo del agente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(agente.Telefono) || string.IsNullOrWhiteSpace(agente.Telefono))
            {
                mensaje = "El Telefono del agente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(agente.Contraseña) || string.IsNullOrWhiteSpace(agente.Contraseña))
            {
                mensaje = "La Contraseña del agente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                //string clave = RecursosService.GenerarClave();
                string asunto = "Creacion de cuenta";
                string mensaje_correo = "<h3> Su cuenta fue creada correctamente </h3></br>";
                //agente.Contraseña = RecursosService.ConvertirSha256(clave);
                bool respuesta = RecursosService.EnviarCorreo(agente.Correo, asunto, mensaje_correo);
                if (respuesta)
                {
                    return objDatos.Registrar(agente, out mensaje);
                }
                else
                {
                    mensaje = "No se pudo enviar el correo";
                    return 0;
                }
                
            }
            else
            {
                return 0;
            }

        }
        public bool Editar(Agente agente, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(agente.Nombre) || string.IsNullOrWhiteSpace(agente.Nombre))
            {
                mensaje = "El nombre del agente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(agente.Apellido) || string.IsNullOrWhiteSpace(agente.Apellido))
            {
                mensaje = "El apellido del agente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(agente.Correo) || string.IsNullOrWhiteSpace(agente.Correo))
            {
                mensaje = "El Correo del agente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(agente.Telefono) || string.IsNullOrWhiteSpace(agente.Telefono))
            {
                mensaje = "El Telefono del agente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(agente.Contraseña) || string.IsNullOrWhiteSpace(agente.Contraseña))
            {
                mensaje = "La Contraseña del agente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return objDatos.Editar(agente, out mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool Eliminar(int cod, out string mensaje)
        {
            return objDatos.Eliminar(cod, out mensaje);
        }
        public Agente GetByCod(int cod)
        {
            return objDatos.GetById(cod);
        }
        public Agente GetByCorreo(string correo)
        {
            return objDatos.GetByCorreo(correo);
        }
    }
}
