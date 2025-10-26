using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AccesoDatosCafe;

namespace LogicaCafe
{
    public class LogicaUsuarios
    {
        private readonly UsuarioDAO usuarioDAO = new UsuarioDAO();

        // ============================================================
        // LISTAR USUARIOS
        // ============================================================
        public List<Usuario> ObtenerUsuarios()
        {
            try
            {
                return usuarioDAO.ObtenerUsuarios();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica al listar usuarios: " + ex.Message);
            }
        }

        // ============================================================
        // REGISTRAR CLIENTE (con validaciones avanzadas y limpieza)
        // ============================================================
        public bool RegistrarCliente(string nombre, string apellido, string email, string password)
        {
            try
            {
                // -------------------------
                // LIMPIEZA DE DATOS
                // -------------------------
                nombre = nombre?.Trim();
                apellido = apellido?.Trim();
                email = email?.Trim().ToLower();
                password = password?.Trim();

                // -------------------------
                // VALIDACIONES BÁSICAS
                // -------------------------
                if (string.IsNullOrWhiteSpace(nombre) ||
                    string.IsNullOrWhiteSpace(apellido) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception("Todos los campos son obligatorios.");
                }

                // -------------------------
                // VALIDAR NOMBRE Y APELLIDO
                // -------------------------
                if (!Regex.IsMatch(nombre, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$"))
                    throw new Exception("El nombre solo puede contener letras y espacios.");

                if (!Regex.IsMatch(apellido, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$"))
                    throw new Exception("El apellido solo puede contener letras y espacios.");

                // -------------------------
                // VALIDAR EMAIL
                // -------------------------
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(email, emailPattern))
                    throw new Exception("El correo electrónico debe tener un formato válido (ejemplo: usuario@correo.com).");

                // -------------------------
                // VALIDAR PASSWORD
                // -------------------------
                if (password.Length < 4)
                    throw new Exception("La contraseña debe tener al menos 4 caracteres.");

                // -------------------------
                // REGISTRO EN BASE DE DATOS
                // -------------------------
                bool resultado = usuarioDAO.CrearUsuario(nombre, apellido, email, password);

                // Limpieza de estados antiguos (por si hubo error previo)
                GC.Collect();
                GC.WaitForPendingFinalizers();

                return resultado;
            }
            catch (Exception ex)
            {
                GC.Collect();
                throw new Exception("Error al registrar cliente: " + ex.Message);
            }
        }

        // ============================================================
        // LOGIN
        // ============================================================
        public Usuario Login(string email, string password)
        {
            try
            {
                email = email?.Trim().ToLower();
                password = password?.Trim();

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    throw new Exception("Debe ingresar credenciales válidas.");

                var usuario = usuarioDAO.Login(email, password);

                if (usuario == null)
                    throw new Exception("Usuario no encontrado o credenciales incorrectas.");

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica de inicio de sesión: " + ex.Message);
            }
        }
    }
}
