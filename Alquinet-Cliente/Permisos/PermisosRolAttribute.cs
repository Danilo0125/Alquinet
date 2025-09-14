using Alquinet_Entidad;
using System;
using System.Web;
using System.Web.Mvc;

public class PermisosRolAttribute : ActionFilterAttribute
{
    private readonly string rol;

    public PermisosRolAttribute(string rol)
    {
        this.rol = rol;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var sessionPersona = HttpContext.Current.Session["Persona"] as Persona;

        if (sessionPersona == null)
        {
            // Redirigir si no hay sesión
            filterContext.Result = new RedirectResult("~/Home/Index");
            return;
        }

        switch (rol.ToLower())
        {
            case "agente":
                if (sessionPersona is Agente)
                {
                    // Permitir la acción
                    return;
                }
                break;
            case "usuario":
                if (sessionPersona is Usuario)
                {
                    // Permitir la acción
                    return;
                }
                break;
            default:
                // Manejo de caso de rol no reconocido
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
        }

        // Redirigir si el rol no coincide
        filterContext.Result = new RedirectResult("~/Home/Index");
    }
}
