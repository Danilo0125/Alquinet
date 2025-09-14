using Alquinet_Datos;
using Alquinet_Entidad;
using Alquinet_Entidad.ClasesPropiedad;
using Alquinet_Entidad.FactoryMethod;
using Alquinet_Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alquinet_Cliente.Controllers
{
    public class PropiedadController : Controller
    {
        [HttpPost]
        public ActionResult Departemento(Departamento departamento)
        {
            return View(departamento);
        }
        [HttpPost]
        public ActionResult Casa(Casa casa)
        {
            return View(casa);
        }
        [HttpGet]
        public ActionResult Mostrar(int cod_propiedad)
        {
            Propiedad propiedad = new PropiedadService().GetById(cod_propiedad);
            if (propiedad != null)
            {
                propiedad = PropiedadFactory.CrearPropiedad(propiedad);
                if (propiedad is Departamento)
                {
                    propiedad.Direccion = new DireccionService().GetById(propiedad.Cod_direccion);
                    propiedad.Fotos = new FotoService().GetFotosByCod(propiedad.Cod);
                    propiedad.Agente = new AgenteService().GetByCod(propiedad.Cod_agente);
                    if (propiedad.Fotos != null && propiedad.Fotos.Any())
                    {
                        foreach (var foto in propiedad.Fotos)
                        {
                            if (System.IO.File.Exists(foto.DireccionFoto))
                            {
                                byte[] imageBytes = System.IO.File.ReadAllBytes(foto.DireccionFoto);
                                foto.Base64 = Convert.ToBase64String(imageBytes);
                            }
                        }
                    }
                    return View("Departamento",propiedad as Departamento); // Suponiendo que tienes una vista llamada "Mostrar" para mostrar la propiedad
                }
                else if(propiedad is Casa)
                {
                    propiedad.Direccion = new DireccionService().GetById(propiedad.Cod_direccion);
                    propiedad.Fotos = new FotoService().GetFotosByCod(propiedad.Cod);
                    propiedad.Agente = new AgenteService().GetByCod(propiedad.Cod_agente);

                    if (propiedad.Fotos != null && propiedad.Fotos.Any())
                    {
                        foreach (var foto in propiedad.Fotos)
                        {
                            if (System.IO.File.Exists(foto.DireccionFoto))
                            {
                                byte[] imageBytes = System.IO.File.ReadAllBytes(foto.DireccionFoto);
                                foto.Base64 = Convert.ToBase64String(imageBytes);
                            }
                        }
                    }
                    return View("Casa", propiedad as Casa); // Suponiendo que tienes una vista llamada "Mostrar" para mostrar la propiedad
                }
            }

            return RedirectToAction("Index", "Home"); // Redirecciona al índice de la página principal si la propiedad no se encontró o no es un departamento
        }

        public ActionResult Index()
        {
            List<Propiedad> propiedads = new PropiedadService().ListarDisponibles();
            foreach(var propiedad in propiedads)
            {
                propiedad.Direccion = new DireccionService().GetById(propiedad.Cod_direccion);
                if (propiedad != null)
                {
                    List<Fotos> fotos = new FotoService().GetFotosByCod(propiedad.Cod);

                    if (fotos != null)
                    {
                        foreach (var foto in fotos)
                        {
                            if (System.IO.File.Exists(foto.DireccionFoto))
                            {
                                byte[] imageBytes = System.IO.File.ReadAllBytes(foto.DireccionFoto);
                                foto.Base64 = Convert.ToBase64String(imageBytes);
                            }
                        }
                        propiedad.Fotos = fotos;
                    }
                }
            }
            return View(propiedads);
        }
        
    }
}