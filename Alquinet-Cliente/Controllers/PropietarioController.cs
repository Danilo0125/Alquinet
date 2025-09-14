using Alquinet_Datos;
using Alquinet_Entidad;
using Alquinet_Entidad.ClasesPropiedad;
using Alquinet_Negocio;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Alquinet_Cliente.Controllers
{
    [PermisosRol("usuario")]
    public class PropietarioController : Controller
    {
        // GET: Propietario
        public ActionResult Departamento()
        {
            return View();
        }
        public ActionResult Casa()
        {
            return View();
        }

        [HttpPost]
        public JsonResult VerificarPrecio(string zona, string area, string tipo, string precio)
        {
            string apiUrl = $"http://192.168.88.254:8000/BonitaPropiedad?zona={zona}&area={area}&tipo={tipo}&precio={precio}";
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    var response = client.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = response.Content.ReadAsStringAsync().Result;
                        dynamic resultado = JsonConvert.DeserializeObject(responseBody);

                        if (resultado != null && resultado.probabilidad != null)
                        {
                            decimal probabilidad = resultado.probabilidad;
                            return Json(new { operacion_exitosa = true, probabilidad });
                        }
                        else
                        {
                            return Json(new { operacion_exitosa = false, mensaje = "No se pudo obtener la probabilidad." });
                        }
                    }
                    else
                    {
                        return Json(new { operacion_exitosa = false, mensaje = "Error al contactar con el servicio de validación de precio." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { operacion_exitosa = false, mensaje = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Guardar(string objDireccion, string objPropiedad, HttpPostedFileBase[] archivosImagen)
        {
            try
            {
                string mensaje;
                bool operacion_exitosa = true;
                bool guardar_imagen_exito = true;

                // Deserialización de los objetos JSON
                Direccion oDireccion = JsonConvert.DeserializeObject<Direccion>(objDireccion);
                Propiedad oPropiedad = JsonConvert.DeserializeObject<Propiedad>(objPropiedad);

                // Asignaciones adicionales a la propiedad
                oPropiedad.Cod_usuario = (Session["Persona"] as Usuario).Cod;
                oPropiedad.Tipo = "Departamento";  // Ajusta si registras otros tipos
                oPropiedad.Disponibilidad = "Pendiente";

                // Validar que los datos de dirección y propiedad sean correctos
                decimal precio;
                if (string.IsNullOrEmpty(oPropiedad.Titulo) || oPropiedad.Area <= 0 || !decimal.TryParse(oPropiedad.PrecioTexto, out precio) || precio <= 0)
                {
                    return Json(new { operacion_exitosa = false, mensaje = "Datos de propiedad inválidos. Verifica que el título, área y precio sean correctos." });
                }
                oPropiedad.Precio = precio;

                // Registrar Dirección
                int codDic = new DireccionService().Registrar(oDireccion, out mensaje);
                if (codDic == -1)
                {
                    return Json(new { operacion_exitosa = false, mensaje = "Error al registrar la dirección: " + mensaje });
                }
                oPropiedad.Cod_direccion = codDic;

                // Registrar Propiedad
                if (oPropiedad.Cod == 0)
                {
                    int id_generado = new PropiedadService().Registrar(oPropiedad, out mensaje);
                    if (id_generado == 0)
                    {
                        return Json(new { operacion_exitosa = false, mensaje = "Error al registrar la propiedad: " + mensaje });
                    }
                    oPropiedad.Cod = id_generado;
                }
                else
                {
                    new PropiedadService().Editar(oPropiedad, out mensaje);
                }

                // Validar imágenes
                if (archivosImagen != null && archivosImagen.Length > 0)
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
                            string extension = Path.GetExtension(archivoImagen.FileName).ToLower();
                            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                            {
                                return Json(new { operacion_exitosa = false, mensaje = "Solo se permiten imágenes en formato JPG, JPEG o PNG." });
                            }

                            string nombre_imagen = $"{oPropiedad.Cod}_{Guid.NewGuid()}{extension}";
                            string ruta_completa = Path.Combine(ruta_guardar, nombre_imagen);

                            try
                            {
                                archivoImagen.SaveAs(ruta_completa);

                                Fotos foto = new Fotos
                                {
                                    Ambiente = "General",  // Ajusta esto según sea necesario
                                    DireccionFoto = ruta_completa,
                                    Cod_propiedad = oPropiedad.Cod
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
                    mensaje += " Se guardó la propiedad, pero hubo problemas al guardar algunas imágenes.";
                }

                return Json(new { operacion_exitosa = true, mensaje = "Propiedad registrada correctamente." + mensaje });
            }
            catch (Exception ex)
            {
                return Json(new { operacion_exitosa = false, mensaje = $"Error al registrar propiedad: {ex.Message}" });
            }
        }

    }

}