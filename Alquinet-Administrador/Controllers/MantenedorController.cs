using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alquinet_Administrador.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Productos()
        {
            return View();
        }
        public ActionResult Visitas()
        {
            return View();
        }
        public ActionResult Alquilados()
        {
            return View();
        }
    }
}