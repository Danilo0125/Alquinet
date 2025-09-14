using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alquinet_Cliente.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Hola_Mundo()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
    }
}