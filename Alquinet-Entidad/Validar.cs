using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alquinet_Entidad
{
    public class Validar
    {
        public static bool ValidarCorreo(string correo)
        {
            return Regex.IsMatch(correo, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }
        public static string ValidarClave(string clave)
        {
            // La contraseña debe tener al menos 8 caracteres
            if (clave.Length < 8)
            {
                return "La contraseña debe tener al menos 8 caracteres.";
            }
            // La contraseña debe contener al menos una letra mayúscula
            if (!clave.Any(char.IsUpper))
            {
                return "La contraseña debe contener al menos una letra mayúscula.";
            }
            // La contraseña debe contener al menos una letra minúscula
            if (!clave.Any(char.IsLower))
            {
                return "La contraseña debe contener al menos una letra minúscula.";
            }
            // La contraseña debe contener al menos un dígito
            if (!clave.Any(char.IsDigit))
            {
                return "La contraseña debe contener al menos un dígito.";
            }
            // La contraseña debe contener al menos un carácter especial
            string caracteresEspeciales = @"!@#$%^&*()-_+=[]{}|;:'""\<>,.?/";
            if (!clave.Any(caracteresEspeciales.Contains))
            {
                return "La contraseña debe contener al menos un carácter especial.";
            }
            return string.Empty; // Si pasa todas las validaciones, se devuelve una cadena vacía
        }
        public static string ValidarFormatoMoneda(string precioText)
        {
            if (Regex.IsMatch(precioText, @"^\d{1,}\.\d{2}$"))
            {
                if (Double.Parse(precioText) > 0)
                {
                    return string.Empty;
                }
                else return "No se acepta a cero ni numero negativos";
            }
            else return "El formato del precio no es válido. Debe ser del tipo ##.##";
        }
        public static string ValidarFormatoArea(string areaText) => Regex.IsMatch(areaText, @"^\d{1,}\.\d{2}$") ? "" : "El formato del area no es válido. Debe ser del tipo ##.##";

    }
}
