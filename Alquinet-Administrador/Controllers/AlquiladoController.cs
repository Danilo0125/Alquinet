using Alquinet_Entidad;
using Alquinet_Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alquinet_Administrador.Controllers
{
    public class AlquiladoController : Controller
    {
        public ActionResult Alquilados()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Listar()
        {
            List<Alquilado> alquilados = new AlquiladoService().Listar();
            return Json(new { data = alquilados }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AlquiladoById(int id)
        {
            return Json(new AlquiladoService().GetById(id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Guardar(Alquilado alquilado)
        {
            object result;
            string mensaje = string.Empty;

            if (alquilado.Cod == 0)
            {
                result = new AlquiladoService().Registrar(alquilado, out mensaje);
            }
            else
            {
                result = new AlquiladoService().Editar(alquilado, out mensaje);
            }
            return Json(new { resultado = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Eliminar(int cod)
        {
            bool result = false;
            string mensaje = string.Empty;
            result = new AlquiladoService().Eliminar(cod, out mensaje);
            return Json(new { resultado = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}
