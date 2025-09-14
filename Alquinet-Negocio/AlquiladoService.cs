using Alquinet_Datos;
using Alquinet_Entidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Alquinet_Negocio
{
    public class AlquiladoService
    {
        private AlquiladoDatos objDatos = new AlquiladoDatos();
        public List<Alquilado> Listar()
        {
            return objDatos.Listar();
        }
        public Alquilado GetById(int id)
        {
            return objDatos.GetById(id);
        }
        public int Registrar(Alquilado alquilado, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(alquilado.Fecha.ToString()) || string.IsNullOrWhiteSpace(alquilado.Fecha.ToString()))
            {
                mensaje = "Porfavor llene la fecha";
            }
            //else if (string.IsNullOrEmpty(alquilado.ComisionText) || string.IsNullOrWhiteSpace(alquilado.ComisionText))
            //{
            //    mensaje = "Porfavor llene el campo comision";
            //}
            else if (double.TryParse(alquilado.ComisionText, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out double precio))
            {
                alquilado.Comision = precio;
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                alquilado.Comision = Convert.ToDouble(alquilado.ComisionText);
                return objDatos.Registrar(alquilado, out mensaje);
            }
            else
            {
                return 0;
            }

        }
        public bool Editar(Alquilado alquilado, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(alquilado.Fecha.ToString()) || string.IsNullOrWhiteSpace(alquilado.Fecha.ToString()))
            {
                mensaje = "Porfavor llene la fecha";
            }
            else if (string.IsNullOrEmpty(alquilado.ComisionText) || string.IsNullOrWhiteSpace(alquilado.ComisionText))
            {
                mensaje = "Porfavor llene el campo comision";
            }
            else if (double.TryParse(alquilado.ComisionText, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out double precio))
            {
                alquilado.Comision = precio;
            }
            alquilado.Comision = Convert.ToDouble(alquilado.ComisionText);

            if (string.IsNullOrEmpty(mensaje))
            {
                return objDatos.Editar(alquilado, out mensaje);
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
