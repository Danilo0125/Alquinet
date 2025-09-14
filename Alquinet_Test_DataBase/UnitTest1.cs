using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alquinet_Datos;
using Alquinet_Entidad;
using Npgsql;

namespace Alquinet_Test_DataBase
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        private readonly string ConnectionString = Alquinet_Datos.Conexion.cn;

        [TestInitialize]
        public void Setup()
        {
            // Borrar la tabla de usuarios antes de cada prueba
            using (NpgsqlConnection con = new NpgsqlConnection(ConnectionString))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Usuario;", con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Usuario usuario = new Usuario
            {
                Nombre = "Danilo",
                Apellido = "Chavez",
                Telefono = "70721092",
                Ci = "9404605",
                Correo = "nanau.chavez@gmail.com",
                Contraseña = "dchavezÑ0125"
            };

        }
        [TestMethod]
        public void Registrar_UsuarioValido_DevuelveIdUsuario()
        {
            // Arrange
            UsuarioDatos usuarioDatos = new UsuarioDatos();
            Usuario usuario = new Usuario
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = "123456789",
                Ci = "123456789",
                Correo = "juan.perez@example.com",
                Contraseña = "password123"
            };
            string mensaje;

            // Act
            int resultado = usuarioDatos.Registrar(usuario, out mensaje);

            // Assert
            Assert.IsTrue(resultado > 0, "El ID del usuario debe ser mayor que 0.");
            Assert.AreEqual("Usuario creado exitosamente", mensaje);
        }
        [TestMethod]
        public void Registrar_CorreoRepetido_DevuelveError()
        {
            // Arrange
            UsuarioDatos usuarioDatos = new UsuarioDatos();
            Usuario usuario1 = new Usuario
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = "123456789",
                Ci = "987654321",
                Correo = "correo.repetido@example.com",
                Contraseña = "password123"
            };
            Usuario usuario2 = new Usuario
            {
                Nombre = "Maria",
                Apellido = "Gomez",
                Telefono = "987654321",
                Ci = "789456123",
                Correo = "correo.repetido@example.com", // Mismo correo
                Contraseña = "password456"
            };
            string mensaje;

            // Act
            bool resultado_true = 0 < usuarioDatos.Registrar(usuario1, out mensaje); // Registrar primer usuario
            bool resultado_false = 0 < usuarioDatos.Registrar(usuario2, out mensaje); // Intentar registrar segundo usuario con el mismo mail

            // Assert
            Assert.IsTrue(resultado_true, "El primer usuario fue registrado correctamente");
            Assert.IsFalse(resultado_false, "El sugundo usuario no fue registrado por Mail ya existe");
            Assert.AreEqual("El correo o el CI del usuario ya existen", mensaje);
        }

        [TestMethod]
        public void Registrar_CIRepetido_DevuelveError()
        {
            // Arrange
            UsuarioDatos usuarioDatos = new UsuarioDatos();
            Usuario usuario1 = new Usuario
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = "123456789",
                Ci = "963852741", // Mismo CI
                Correo = "juan.perez2@example.com",
                Contraseña = "password123"
            };
            Usuario usuario2 = new Usuario
            {
                Nombre = "Maria",
                Apellido = "Gomez",
                Telefono = "123456789",
                Ci = "963852741", // Mismo CI
                Correo = "maria.gomez@example.com",
                Contraseña = "password456"
            };
            string mensaje;

            // Act
            bool resultado_true = 0 < usuarioDatos.Registrar(usuario1, out mensaje); // Registrar primer usuario
            bool resultado_false = 0 < usuarioDatos.Registrar(usuario2, out mensaje); // Intentar registrar segundo usuario con el mismo CI
            // Assert
            Assert.IsTrue(resultado_true, "El primer usuario fue registrado correctamente");
            Assert.IsFalse(resultado_false, "El sugundo usuario no fue registrado por CI ya existe");
            Assert.AreEqual("El correo o el CI del usuario ya existen", mensaje);
        }
        [TestMethod]
        public void EditarUsuario()
        {
            UsuarioDatos usuarioDatos = new UsuarioDatos();
            Usuario usuario = new Usuario
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = "123456789",
                Ci = "123456789",
                Correo = "juan.perez@example.com",
                Contraseña = "password123"
            };
            string mensaje;
            usuario.Cod=usuarioDatos.Registrar(usuario, out mensaje);
            usuario.Nombre = "Danilo";
            usuario.Contraseña = "ñ_ñ_ñ_ñ_";
            bool test = usuarioDatos.Editar(usuario, out mensaje);
            Assert.IsTrue(test, " El usuario fue editado correctamente con su codigo " + usuario.Cod);
            
        }
        [TestMethod]
        public void EditarUsuario_Ci_repetido()
        {
            UsuarioDatos usuarioDatos = new UsuarioDatos();
            Usuario usuario1 = new Usuario
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = "123456789",
                Ci = "123456789",
                Correo = "juan.perez@example.com",
                Contraseña = "password123"
            };
            Usuario usuario2 = new Usuario
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = "123456780",
                Ci = "123456780",
                Correo = "juan.pe@example.com",
                Contraseña = "password123"
            };
            string mensaje;
            usuario1.Cod = usuarioDatos.Registrar(usuario1, out mensaje);
            usuario2.Cod = usuarioDatos.Registrar(usuario2, out mensaje);

            //Asignacion de de gmail repetido
            usuario1.Correo = usuario2.Correo;
            bool test = usuarioDatos.Editar(usuario1, out mensaje);
            Assert.IsTrue(false, " El usuario no se puedo registrar por ci repetido con su codigo " + usuario1.Cod);
        }
        [TestMethod]
        public void EditarUsuario_Email_repetido()
        {
            UsuarioDatos usuarioDatos = new UsuarioDatos();
            Usuario usuario1 = new Usuario
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = "123456789",
                Ci = "123456789",
                Correo = "juan.perez@example.com",
                Contraseña = "password123"
            };
            Usuario usuario2 = new Usuario
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = "123456780",
                Ci = "123456780",
                Correo = "juan.pe@example.com",
                Contraseña = "password123"
            };
            string mensaje;
            usuario1.Cod = usuarioDatos.Registrar(usuario1, out mensaje);
            usuario2.Cod = usuarioDatos.Registrar(usuario2, out mensaje);

            //Asignacion de de gmail repetido
            usuario1.Correo = usuario2.Correo;
            bool test = usuarioDatos.Editar(usuario1, out mensaje);
            Assert.IsTrue(false, " El usuario no se puedo registrar por ci repetido con su codigo " + usuario1.Cod);
        }
    }
}
