using Alquinet_Entidad;
using System;
using System.Collections.Generic;

namespace Alquinet_Datos
{
    public class DireccionService
    {
        private DireccionRepository objDatos = new DireccionRepository();

        public List<Direccion> Listar()
        {
            return objDatos.Listar();
        }

        public Direccion GetById(int id)
        {
            return objDatos.GetById(id);
        }

        public int Registrar(Direccion direccion, out string mensaje)
        {
            mensaje = string.Empty;

            // Validaciones básicas
            if (string.IsNullOrEmpty(direccion.Cod_pos) || string.IsNullOrWhiteSpace(direccion.Cod_pos))
            {
                mensaje = "Por favor, llene el campo código postal.";
            }
            else if (string.IsNullOrEmpty(direccion.Calle) || string.IsNullOrWhiteSpace(direccion.Calle))
            {
                mensaje = "Por favor, llene el campo calle.";
            }
            else if (string.IsNullOrEmpty(direccion.Barrio) || string.IsNullOrWhiteSpace(direccion.Barrio))
            {
                mensaje = "Por favor, llene el campo barrio.";
            }
            else if (string.IsNullOrEmpty(direccion.Zona) || string.IsNullOrWhiteSpace(direccion.Zona))
            {
                mensaje = "Por favor, llene el campo zona.";
            }
            else if (direccion.Longitud == 0)
            {
                mensaje = "Por favor, llene el campo longitud.";
            }
            else if (direccion.Latitud == 0)
            {
                mensaje = "Por favor, llene el campo latitud.";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return objDatos.Registrar(direccion, out mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Direccion direccion, out string mensaje)
        {
            mensaje = string.Empty;

            // Validaciones básicas
            if (string.IsNullOrEmpty(direccion.Cod_pos) || string.IsNullOrWhiteSpace(direccion.Cod_pos))
            {
                mensaje = "Por favor, llene el campo código postal.";
            }
            else if (string.IsNullOrEmpty(direccion.Calle) || string.IsNullOrWhiteSpace(direccion.Calle))
            {
                mensaje = "Por favor, llene el campo calle.";
            }
            else if (string.IsNullOrEmpty(direccion.Barrio) || string.IsNullOrWhiteSpace(direccion.Barrio))
            {
                mensaje = "Por favor, llene el campo barrio.";
            }
            else if (string.IsNullOrEmpty(direccion.Zona) || string.IsNullOrWhiteSpace(direccion.Zona))
            {
                mensaje = "Por favor, llene el campo zona.";
            }
            else if (direccion.Longitud == 0)
            {
                mensaje = "Por favor, llene el campo longitud.";
            }
            else if (direccion.Latitud == 0)
            {
                mensaje = "Por favor, llene el campo latitud.";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return objDatos.Editar(direccion, out mensaje);
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
