using Alquinet_Datos;
using Alquinet_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alquinet_Negocio
{
    public class PropiedadService
    {
        private PropiedadDatos objDatos = new PropiedadDatos();

        public List<Propiedad> Listar()
        {
            return objDatos.Listar();
        }
        public List<Propiedad> ListarDisponibles()
        {
            return objDatos.ListarDisponibles();
        }

        public Propiedad GetById(int id)
        {
            return objDatos.GetById(id);
        }

        public int Registrar(Propiedad propiedad, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(propiedad.Titulo) || string.IsNullOrWhiteSpace(propiedad.Titulo))
            {
                mensaje = "Por favor, llene el título";
            }
            else if (propiedad.Area <= 0)
            {
                mensaje = "Por favor, ingrese un área válida";
            }
            else if (string.IsNullOrEmpty(propiedad.Tipo) || string.IsNullOrWhiteSpace(propiedad.Tipo))
            {
                mensaje = "Por favor, llene el tipo de propiedad";
            }
            else if (string.IsNullOrEmpty(propiedad.Descripcion) || string.IsNullOrWhiteSpace(propiedad.Descripcion))
            {
                mensaje = "Por favor, llene la descripción";
            }
            else if (string.IsNullOrEmpty(propiedad.Disponibilidad.ToString()) || string.IsNullOrWhiteSpace(propiedad.Disponibilidad.ToString()))
            {
                mensaje = "Por favor, llene la disponibilidad";
            }
            else if (propiedad.Precio <= 0)
            {
                mensaje = "Por favor, ingrese un precio válido";
            }
            else if (propiedad.Cod_usuario <= 0)
            {
                mensaje = "Por favor, ingrese un código de usuario válido";
            }
            //else if (propiedad.Cod_agente <= 0)
            //{
            //    mensaje = "Por favor, ingrese un código de agente válido";
            //}
            else if (propiedad.Cod_direccion <= 0)
            {
                mensaje = "Por favor, ingrese un código de dirección válido";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return objDatos.Registrar(propiedad, out mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Propiedad propiedad, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(propiedad.Titulo) || string.IsNullOrWhiteSpace(propiedad.Titulo))
            {
                mensaje = "Por favor, llene el título";
            }
            else if (propiedad.Area <= 0)
            {
                mensaje = "Por favor, ingrese un área válida";
            }
            else if (string.IsNullOrEmpty(propiedad.Tipo) || string.IsNullOrWhiteSpace(propiedad.Tipo))
            {
                mensaje = "Por favor, llene el tipo de propiedad";
            }
            else if (string.IsNullOrEmpty(propiedad.Descripcion) || string.IsNullOrWhiteSpace(propiedad.Descripcion))
            {
                mensaje = "Por favor, llene la descripción";
            }
            else if (string.IsNullOrEmpty(propiedad.Disponibilidad.ToString()) || string.IsNullOrWhiteSpace(propiedad.Disponibilidad.ToString()))
            {
                mensaje = "Por favor, llene la disponibilidad";
            }
            else if (propiedad.Precio <= 0)
            {
                mensaje = "Por favor, ingrese un precio válido";
            }
            else if (propiedad.Cod_usuario <= 0)
            {
                mensaje = "Por favor, ingrese un código de usuario válido";
            }
            else if (propiedad.Cod_agente <= 0)
            {
                mensaje = "Por favor, ingrese un código de agente válido";
            }
            else if (propiedad.Cod_direccion <= 0)
            {
                mensaje = "Por favor, ingrese un código de dirección válido";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return objDatos.Editar(propiedad, out mensaje);
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
    }
}
