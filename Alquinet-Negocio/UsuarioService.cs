using Alquinet_Datos;
using Alquinet_Entidad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace Alquinet_Negocio
{
    public class UsuarioService
    {
        private UsuarioDatos objDatos = new UsuarioDatos();
        public List<Usuario> Listar()
        {
            return objDatos.Listar();
        }
        public Usuario UsuarioById(int id)
        {
            return objDatos.GetById(id);
        }
        public int Registrar(Usuario usuario, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Nombre))
            {
                mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(usuario.Apellido) || string.IsNullOrWhiteSpace(usuario.Apellido))
            {
                mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrWhiteSpace(usuario.Correo))
            {
                mensaje = "El Correo del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(usuario.Telefono) || string.IsNullOrWhiteSpace(usuario.Telefono))
            {
                mensaje = "El Telefono del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(usuario.Contraseña) || string.IsNullOrWhiteSpace(usuario.Contraseña))
            {
                mensaje = "La Contraseña del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                string clave = RecursosService.GenerarClave();
                string asunto = "Creacion de cuenta";
                string mensaje_correo = "<h3> Su cuenta fue creada correctamente </h3></br>" + clave;
                bool respuesta = RecursosService.EnviarCorreo(usuario.Correo, asunto, mensaje_correo);
                if (respuesta)
                {
                    return objDatos.Registrar(usuario, out mensaje);
                }
                else
                {
                    mensaje = "No se pudo enviar el correo";
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
        public bool Editar(Usuario usuario, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Nombre))
            {
                mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(usuario.Apellido) || string.IsNullOrWhiteSpace(usuario.Apellido))
            {
                mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrWhiteSpace(usuario.Correo))
            {
                mensaje = "El Correo del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(usuario.Telefono) || string.IsNullOrWhiteSpace(usuario.Telefono))
            {
                mensaje = "El Telefono del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(usuario.Contraseña) || string.IsNullOrWhiteSpace(usuario.Contraseña))
            {
                mensaje = "La Contraseña del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return objDatos.Editar(usuario, out mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool Eliminar(int cod, out  string mensaje)
        {
            return objDatos.Eliminar(cod,out mensaje);
        }
        private static readonly HttpClient _httpClient = new HttpClient();
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var response = await _httpClient.GetAsync("http://127.0.0.1:5000/usuario");
            List<Usuario> usuarios = new List<Usuario>();
            if(response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonData);
            }
            else
            {
                return usuarios = new List<Usuario>();
            }
            return usuarios;
        }

        public Usuario GetClientByCorreo(string correo)
        {
            return new UsuarioDatos().GetByCorreo(correo);
        }
    }
}
