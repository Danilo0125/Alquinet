using Alquinet_Datos;
using Alquinet_Entidad;
using Alquinet_Negocio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Alquinet_Administrador.Controllers
{
    public class PropiedadController : Controller
    {

        [HttpGet]
        public JsonResult Listar()
        {
            List<Propiedad> propiedades = new PropiedadService().Listar();
            return Json(new { data = propiedades }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Guardar(string objDireccion, string objPropiedad, HttpPostedFileBase[] archivosImagen)
        {
            string mensaje = string.Empty;
            bool guardar_imagen_exito = true;
            bool operacion_exitosa = true;

            Propiedad oPropiedad = JsonConvert.DeserializeObject<Propiedad>(objPropiedad);
            Direccion oDireccion = JsonConvert.DeserializeObject<Direccion>(objDireccion);

            // Guardar Dirección
            var codDic = new DireccionService().Registrar(oDireccion, out mensaje);
            if (codDic == -1)
            {
                return Json(new { operacion_exitosa = false, mensaje = mensaje });
            }
            else
            {
                oPropiedad.Cod_direccion = codDic;
            }

            // Validar Precio
            if (decimal.TryParse(oPropiedad.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out decimal precio))
            {
                oPropiedad.Precio = precio;
            }
            else
            {
                return Json(new { operacion_exitosa = false, mensaje = "El formato del precio debe ser ##.##" });
            }

            // Registrar Propiedad
            if (oPropiedad.Cod == 0)
            {
                int id_generado = new PropiedadService().Registrar(oPropiedad, out mensaje);
                if (id_generado != 0)
                {
                    oPropiedad.Cod = id_generado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                new PropiedadService().Editar(oPropiedad, out mensaje);
            }

            // Guardar Imágenes
            if (operacion_exitosa && archivosImagen != null && archivosImagen.Length > 0)
            {
                if (archivosImagen.Length > 10)
                {
                    return Json(new { operacion_exitosa = false, mensaje = "Se permite un máximo de 10 imágenes." });
                }

                string ruta_guardar = ConfigurationManager.AppSettings["ImagesPath"];

                foreach (var archivoImagen in archivosImagen)
                {
                    if (archivoImagen != null && archivoImagen.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(archivoImagen.FileName);
                        string nombre_imagen = $"{oPropiedad.Cod}_{Guid.NewGuid()}{extension}";
                        string ruta_completa = Path.Combine(ruta_guardar, nombre_imagen);

                        try
                        {
                            archivoImagen.SaveAs(ruta_completa);

                            Fotos foto = new Fotos
                            {
                                Ambiente = "General", // Ajusta esto según sea necesario
                                DireccionFoto = ruta_completa,
                                //Base64 = Convert.ToBase64String(File.ReadAllBytes(ruta_completa)),
                                Cod_propiedad = oPropiedad.Cod,
                                //Extencion = extension
                            };

                            new FotoService().Registrar(foto, out mensaje);
                        }
                        catch (Exception ex)
                        {
                            mensaje = ex.Message;
                            guardar_imagen_exito = false;
                            break;
                        }
                    }
                }
            }

            if (!guardar_imagen_exito)
            {
                mensaje = "Se guardó el producto pero hubo problemas con las imágenes.";
            }

            return Json(new { operacion_exitosa = operacion_exitosa, id_generado = oPropiedad.Cod, mensaje = mensaje });
        }
        //[HttpPost]
        //public JsonResult Eliminar(int cod)
        //{
        //    bool result = false;
        //    string mensaje = string.Empty;
        //    result = new AlquiladoService().Eliminar(cod, out mensaje);
        //    return Json(new { resultado = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public JsonResult ObtenerPropiedadConDetalles(int cod)
        {
            Propiedad propiedad = new PropiedadService().GetById(cod);
            if (propiedad != null)
            {
                propiedad.Direccion = new DireccionService().GetById(propiedad.Cod_direccion);
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

                return Json(new { operacion_exitosa = true, datos = propiedad }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { operacion_exitosa = false, mensaje = "Propiedad no encontrada" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BonitaPropiedad()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Editar(Propiedad propiedad)
        {
            string mensaje = string.Empty;
            bool operacion_exitosa = true;

            // Deserializar la propiedad desde el JSON recibido
            //Propiedad oPropiedad = JsonConvert.DeserializeObject<Propiedad>(propiedad);

            // Validar Precio
            if (decimal.TryParse(propiedad.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out decimal precio))
            {
                propiedad.Precio = precio;
            }
            else
            {
                return Json(new { operacion_exitosa = false, mensaje = "El formato del precio debe ser ##.##" });
            }

            // Llamar al servicio para editar la propiedad
            try
            {
                new PropiedadService().Editar(propiedad, out mensaje);
            }
            catch (Exception ex)
            {
                operacion_exitosa = false;
                mensaje = ex.Message;
            }

            return Json(new { operacion_exitosa = operacion_exitosa, mensaje = mensaje });
        }

    }
}