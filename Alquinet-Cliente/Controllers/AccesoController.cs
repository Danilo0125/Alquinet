using Alquinet_Entidad;
using Alquinet_Negocio;
using System.Web.Mvc;
using System.Web.Security;

namespace Alquinet_Cliente.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Index (Login Page)
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // POST: Index (Login Process)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string correo, string clave)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(clave))
            {
                ViewBag.ErrorMessage = "Por favor, ingrese un correo electrónico y una contraseña.";
                return View();
            }

            if (!Validar.ValidarCorreo(correo))
            {
                ViewBag.ErrorMessage = "El correo electrónico no es válido.";
                return View();
            }

            var usuario = AuthenticateUser(correo, clave);
            if (usuario != null)
            {
                SetAuthCookie(usuario.Cod.ToString());
                Session["Persona"] = usuario;
                Session["NombreReal"] = usuario.Nombre; // Guardamos el nombre del usuario en la sesión
                return usuario is Agente
                    ? RedirectToAction("Index", "Agente")
                    : RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Correo o contraseña incorrectos.";
            return View();
        }

        // Logout
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session.Clear();  // Clears all session data
            return RedirectToAction("Index", "Home");
        }

        // GET: Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register (Create Account)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Usuario usuario, string clave)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            string errorMsg = Validar.ValidarClave(usuario.Contraseña);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                ModelState.AddModelError("Contraseña", errorMsg);
                return View(usuario);
            }

            if (clave != usuario.Contraseña)
            {
                ModelState.AddModelError("Contraseña", "Las contraseñas deben coincidir.");
                return View(usuario);
            }

            string message;
            var service = new UsuarioService();
            int cod = service.Registrar(usuario, out message);

            if (cod > 0)
            {
                SetAuthCookie(usuario.Cod.ToString());
                Session["Persona"] = usuario;
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, message);
            return View(usuario);
        }

        // Helper Methods

        private Persona AuthenticateUser(string correo, string clave)
        {
            // First, try to get a Usuario
            Persona persona = new UsuarioService().GetClientByCorreo(correo);

            if (persona == null)
            {
                // If no Usuario found, try to get an Agente
                persona = new AgenteService().GetByCorreo(correo);
            }

            // Now, persona could be either a Usuario or an Agente, or still null if not found.


            if (persona != null && persona.Contraseña == clave)  // Replace with secure password comparison
            {
                return persona;
            }
            return null;
        }

        private void SetAuthCookie(string userId)
        {
            FormsAuthentication.SetAuthCookie(userId, false);
        }
    }
}
