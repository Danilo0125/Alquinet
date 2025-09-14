using Alquinet_Entidad.ClasesPropiedad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alquinet_Entidad.FactoryMethod
{
    public interface IPropiedadFactory
    {
        Propiedad CrearPropiedad(Propiedad propiedad);
    }

    public class CasaFactory : IPropiedadFactory
    {
        public Propiedad CrearPropiedad(Propiedad propiedad)
        {
            var partes = propiedad.Descripcion.Split(';');
            if (partes.Length == 2)
            {
                return new Casa
                {
                    Cod = propiedad.Cod,
                    Titulo = propiedad.Titulo,
                    Area = propiedad.Area,
                    Tipo = propiedad.Tipo,
                    Disponibilidad = propiedad.Disponibilidad,
                    Descripcion = partes[1],
                    Precio = propiedad.Precio,
                    PrecioTexto = propiedad.PrecioTexto,
                    Cod_usuario = propiedad.Cod_usuario,
                    Cod_agente = propiedad.Cod_agente,
                    Cod_direccion = propiedad.Cod_direccion,
                    Pisos = partes[0]
                };
            }
            throw new ArgumentException("Descripción inválida para Casa");
        }
    }

    public class DepartamentoFactory : IPropiedadFactory
    {
        public Propiedad CrearPropiedad(Propiedad propiedad)
        {
            /*var partes = propiedad.Descripcion.Split(';');
            if (partes.Length == 3)*/
            if(true)
            {
                return new Departamento
                {
                    Cod = propiedad.Cod,
                    Titulo = propiedad.Titulo,
                    Area = propiedad.Area,
                    Tipo = propiedad.Tipo,
                    Disponibilidad = propiedad.Disponibilidad,
                    Descripcion = propiedad.Descripcion,//partes[2],
                    Precio = propiedad.Precio,
                    PrecioTexto = propiedad.PrecioTexto,
                    Cod_usuario = propiedad.Cod_usuario,
                    Cod_agente = propiedad.Cod_agente,
                    Cod_direccion = propiedad.Cod_direccion,
                    NombreEdificio = "Nombre Edificio",
                    Planta = "2"//partes[1]
                };
            }
            throw new ArgumentException("Descripción inválida para Departamento");
        }
    }
    public abstract class PropiedadFactory
    {
        public static Propiedad CrearPropiedad(Propiedad propiedad)
        {
            var partes = propiedad.Descripcion.Split(';');

            IPropiedadFactory factory;

           /* if (partes.Length == 2)
            {
                factory = new CasaFactory();
            }
            else if (partes.Length == 3)
            {*/
                factory = new DepartamentoFactory();
            /*}
            else
            {
                throw new ArgumentException("Descripción inválida para la propiedad");
            }*/

            return factory.CrearPropiedad(propiedad);
        }
    }
}
