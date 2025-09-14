using Alquinet_Datos;
using Alquinet_Entidad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alquinet_Test_DataBase
{
    /// <summary>
    /// Summary description for RecuperacionAgentes
    /// </summary>
    [TestClass]
    public class RecuperacionAgentes
    {
        private readonly string ConnectionString = Alquinet_Datos.Conexion.cn;

        [TestInitialize]
        public void Setup()
        {
            // Borrar la tabla de agentes antes de cada prueba
            using (NpgsqlConnection con = new NpgsqlConnection(ConnectionString))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Agente;", con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        [TestMethod]
        public void Registrar_UsuarioValido_DevuelveIdUsuario()
        {
            // Arrange
            AgenteDatos agenteDatos = new AgenteDatos();
            Agente agente = new Agente
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
            int resultado = agenteDatos.Registrar(agente, out mensaje);

            // Assert
            Assert.IsTrue(resultado > 0, "El ID del usuario debe ser mayor que 0.");
            Assert.AreEqual("Usuario creado exitosamente", mensaje);
        }
        [TestMethod]
        public void Registrar_CorreoRepetido_DevuelveError()
        {
            // Arrange
            UsuarioDatos agenteDatos = new UsuarioDatos();
            Usuario agente1 = new Usuario
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
            bool resultado_true = 0 < agenteDatos.Registrar(agente1, out mensaje); // Registrar primer usuario
            bool resultado_false = 0 < agenteDatos.Registrar(usuario2, out mensaje); // Intentar registrar segundo usuario con el mismo mail

            // Assert
            Assert.IsTrue(resultado_true, "El primer usuario fue registrado correctamente");
            Assert.IsFalse(resultado_false, "El sugundo usuario no fue registrado por Mail ya existe");
            Assert.AreEqual("El correo o el CI del usuario ya existen", mensaje);
        }

        [TestMethod]
        public void Registrar_CIRepetido_DevuelveError()
        {
            // Arrange
            UsuarioDatos agenteDatos = new UsuarioDatos();
            Usuario agente1 = new Usuario
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = "123456789",
                Ci = "963852741", // Mismo CI
                Correo = "juan.perez2@example.com",
                Contraseña = "password123"
            };
            Usuario agente2 = new Usuario
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
            bool resultado_true = 0 < agenteDatos.Registrar(agente1, out mensaje); // Registrar primer usuario
            bool resultado_false = 0 < agenteDatos.Registrar(agente2, out mensaje); // Intentar registrar segundo usuario con el mismo CI
            // Assert
            Assert.IsTrue(resultado_true, "El primer usuario fue registrado correctamente");
            Assert.IsFalse(resultado_false, "El sugundo usuario no fue registrado por CI ya existe");
            Assert.AreEqual("El correo o el CI del usuario ya existen", mensaje);
        }
    }
}
