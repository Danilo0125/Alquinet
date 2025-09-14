using Alquinet_Entidad;
using Alquinet_Negocio;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Alquinet_Cliente.Controllers
{
    [PermisosRol("agente")] // Agente
    public class AgenteController : Controller
    {
        // GET: Agente
        public ActionResult Index()
        {
            return View();
        }

        // Lista todas las reservas pendientes
        [HttpGet]
        public JsonResult ListarPendientes()
        {
            Agente agente = Session["Persona"] as Agente;
            List<Visita> visitas = new VisitaService().Listar(agente.Cod, "Reservado");
            return Json(new { data = visitas }, JsonRequestBehavior.AllowGet);
        }

        // Lista todas las reservas confirmadas
        [HttpGet]
        public JsonResult ListarConfirmados()
        {
            Agente agente = Session["Persona"] as Agente;
            List<Visita> visitas = new VisitaService().Listar(agente.Cod, "Confirmado");
            return Json(new { data = visitas }, JsonRequestBehavior.AllowGet);
        }

        // Obtiene una reserva por su ID
        [HttpGet]
        public JsonResult VisitaById(int id)
        {
            return Json(new VisitaService().GetById(id), JsonRequestBehavior.AllowGet);
        }

        // Guarda una nueva reserva o edita una existente
        [HttpPost]
        public JsonResult Guardar(Visita visita)
        {
            object result;
            string mensaje = string.Empty;

            if (visita.Cod == 0) // Si el código es 0, es una nueva reserva
            {
                visita.Cod_agente = (Session["Persona"] as Agente).Cod;
                visita.Fecha = DateTime.Now;
                result = new VisitaService().Registrar(visita, out mensaje);
            }
            else // De lo contrario, es una edición
            {
                visita.Cod_agente = (Session["Persona"] as Agente).Cod;
                visita.Fecha = DateTime.Now;
                result = new VisitaService().Editar(visita, out mensaje);
            }
            return Json(new { resultado = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // Elimina una reserva por su ID
        [HttpPost]
        public JsonResult Eliminar(int cod)
        {
            bool result = false;
            string mensaje = string.Empty;
            result = new VisitaService().Eliminar(cod, out mensaje);
            return Json(new { resultado = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // Confirma una visita
        [HttpPost]
        public JsonResult ConfirmarVisita(int cod)
        {
            Visita visita = new VisitaService().GetById(cod);
            if (visita != null)
            {
                visita.Estado = "Confirmado";
                bool result = new VisitaService().Editar(visita, out string mensaje); // Asumiendo que tienes un método para editar en tu servicio
                return Json(new { resultado = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { resultado = false, mensaje = "Visita no encontrada" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Alquilar(int cod)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            try
            {
                // Obtener la visita confirmada por su código
                Visita visita = new VisitaService().GetById(cod);
                if (visita != null && visita.Estado == "Confirmado")
                {
                    // Crear una nueva instancia de Alquilado
                    Alquilado alquilado = new Alquilado
                    {
                        Fecha = DateTime.Now,
                        Cod_usuario = visita.Cod_usuario,
                        Cod_agente = visita.Cod_agente,
                        Cod_propiedad = visita.Cod_propiedad,
                        Comision = CalcularComision(200) // Método para calcular la comisión
                    };

                    // Guardar en la base de datos
                    resultado = new AlquiladoService().Registrar(alquilado, out mensaje) > 0;
                }
                else
                {
                    mensaje = "Visita no confirmada o no encontrada.";
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error: " + ex.Message;
            }

            return Json(new { resultado, mensaje }, JsonRequestBehavior.AllowGet);
        }

        // Método para calcular la comisión (puede ser una lógica fija o basada en algún criterio)
        private double CalcularComision(int codPropiedad)
        {
            // Implementar la lógica de cálculo de comisión según tu negocio
            return 100.0; // Ejemplo de comisión fija
        }
    }
}
