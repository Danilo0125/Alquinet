/*using Alquinet_Entidad;
using Alquinet_Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Security;

namespace Alquinet_Cliente.Controllers
{
    public class AccesoController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        // GET: Acceso
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            if (string.IsNullOrEmpty(clave) || string.IsNullOrWhiteSpace(correo))
            {
                ViewBag.ErrorMessage = "Por favor, ingrese un correo electrónico y una contraseña.";
                return View();
            }
            if (!Validar.ValidarCorreo(correo))
            {
                ViewBag.ErrorMessage = "El correo electrónico no es válido.";
                return View();
            }

            Persona objeto = new UsuarioService().GetClientByCorreo(correo);
            if (objeto != null)
            {
                if (objeto.Contraseña == clave)
                {
                    FormsAuthentication.SetAuthCookie(objeto.Cod.ToString(), false);
                    Session["Persona"] = objeto;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "La contraseña es incorrecta.";
                    return View();
                }
            }
            else
            {
                objeto = new AgenteService().GetByCorreo(correo);
                if (objeto != null)
                {
                    if (objeto.Contraseña == clave)
                    {
                        FormsAuthentication.SetAuthCookie(objeto.Cod.ToString(), false);
                        Session["Persona"] = objeto;
                        return RedirectToAction("Index", "Agente");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Contraseña del agente Incorrecta";
                        return View();
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "El usuario no existe.";
                    return View();
                }
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session["Persona"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Usuario objeto, string clave)
        {
            if (ModelState.IsValid)
            {
                string mensajeError = Validar.ValidarClave(objeto.Contraseña);
                // Validar la contraseña
                if (!string.IsNullOrEmpty(mensajeError))
                {
                    ModelState.AddModelError("Contraseña", mensajeError);
                }
                else if (clave != objeto.Contraseña)
                {
                    ModelState.AddModelError("Contraseña", "Las contraseñas deben coincidir");
                }
                else
                {
                    string mensaje = string.Empty;
                    UsuarioService service = new UsuarioService();
                    int cod = service.Registrar(objeto, out mensaje);
                    if (cod > 0)
                    {
                        FormsAuthentication.SetAuthCookie(objeto.Cod.ToString(), false);
                        Session["Persona"] = objeto;
                        return RedirectToAction("Index", "Home");
                    }
                    ViewBag.Error = mensaje;
                }
            }
            return View(objeto);
        }

    }
}*/