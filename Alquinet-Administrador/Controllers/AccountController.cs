using System.Web.Mvc;

public class AccountController : Controller
{
    // Acción para mostrar el formulario de login
    public ActionResult Login()
    {
        return View();
    }

    // Acción para manejar la autenticación
    [HttpPost]
    public ActionResult Login(string username, string password)
    {
        // Verifica si el nombre de usuario es "admin" y la contraseña es "12345"
        if (username == "admin" && password == "12345")
        {
            // Autenticación exitosa, crea una cookie de autenticación
            System.Web.Security.FormsAuthentication.SetAuthCookie(username, false);

            // Redirige a la página principal o a la página de administración
            return RedirectToAction("Index", "Home");
        }

        // Si la autenticación falla, muestra un mensaje de error
        ViewBag.ErrorMessage = "Nombre de usuario o contraseña incorrectos";
        return View();
    }

    // Acción para cerrar sesión
    public ActionResult Logout()
    {
        System.Web.Security.FormsAuthentication.SignOut();
        return RedirectToAction("Login");
    }
}
