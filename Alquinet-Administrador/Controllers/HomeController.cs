using Alquinet_Entidad;
using Alquinet_Datos;
using System.Collections.Generic;
using System.Web.Mvc;
using Alquinet_Negocio;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Build.Utilities;
using System.Windows.Documents;

namespace Alquinet_Administrador.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private static readonly UsuarioService usuarioService = new UsuarioService();

        //public async Task<ActionResult> Index()
        //{
        //    List<Usuario> usuarios = await usuarioService.GetUsuariosAsync();

        //    return View(usuarios);
        //}
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Agentes()
        {
            return View();
        }
        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaUsuarios()
        {
            List<Usuario> usuarios = new UsuarioService().Listar();
            return Json(new { data = usuarios }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UsuarioById(int id)
        {
            return Json(new UsuarioService().UsuarioById(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListaAgentes()
        {
            List<Agente> agentes = new AgenteService().Listar();
            return Json(new { data = agentes }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarAgentes(Agente agente)
        {
            object result;
            string mensaje = string.Empty;

            if (agente.Cod == 0)
            {
                result = new AgenteService().Registrar(agente, out mensaje);
            }
            else
            {
                result = new AgenteService().Editar(agente, out mensaje);
            }
            return Json(new {resultado = result, mensaje = mensaje}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarUsuarios(Usuario usuario)
        {
            object result;
            string mensaje = string.Empty;
            if(usuario.Cod == 0)
            {
                result = new UsuarioService().Registrar(usuario, out mensaje);
            }
            else
            {
                result = new UsuarioService().Editar(usuario, out mensaje);
            }
            return Json(new { resultado = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarAgente(int cod)
        {
            bool result = false;
            string mensaje = string.Empty;
            result = new AgenteService().Eliminar(cod, out mensaje);
            return Json(new { resultado = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarUsuario(int cod)
        {
            bool result = false;
            string mensaje = string.Empty;
            result = new UsuarioService().Eliminar(cod, out mensaje);
            return Json(new { resultado = result,mensaje = mensaje}, JsonRequestBehavior.AllowGet);
        }

    }
}