using AccesoDatosCafe;
using LogicaCafe;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Services;



namespace WS_GestionCafe
{
    [WebService(Namespace = "http://cafeteria.com/gestion")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Gestion : WebService
    {
        // ==========================
        // CAPAS LÓGICAS Y DAO
        // ==========================
        private readonly LogicaUsuarios logicaUsuarios = new LogicaUsuarios();
        private readonly LogicaTiposServicio logicaTipos = new LogicaTiposServicio();
        private readonly CarritoDAO carritoDAO = new CarritoDAO();

        // ============================================================
        // SECCIÓN 1: USUARIOS
        // ============================================================
        [WebMethod(Description = "Lista todos los usuarios del sistema")]
        public List<Usuario> ObtenerUsuarios()
        {
            try
            {
                return logicaUsuarios.ObtenerUsuarios();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener usuarios: " + ex.Message);
            }
        }

        // ============================================================
        // SECCIÓN 2: TIPOS DE SERVICIO
        // ============================================================
        [WebMethod(Description = "Lista de tipos de servicio (café, postres, etc.)")]
        public List<TipoServicio> ObtenerTiposServicio()
        {
            try
            {
                return logicaTipos.ObtenerTipos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener tipos de servicio: " + ex.Message);
            }
        }

        // ============================================================
        // SECCIÓN 3: CARRITO DE COMPRAS
        // ============================================================
        [WebMethod(Description = "Crea un nuevo carrito de compras para el usuario")]
        public int CrearCarrito(int usuarioId)
        {
            try
            {
                return carritoDAO.CrearCarrito(usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el carrito: " + ex.Message);
            }
        }

        [WebMethod(Description = "Agrega un producto al carrito")]
        public bool AgregarItemCarrito(int carritoId, int productoId, int cantidad, decimal precioUnit)
        {
            try
            {
                return carritoDAO.AgregarItem(carritoId, productoId, cantidad, precioUnit);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar producto al carrito: " + ex.Message);
            }
        }

        [WebMethod(Description = "Confirma el carrito y cambia su estado a CONFIRMADO")]
        public bool ConfirmarCarrito(int carritoId, decimal total)
        {
            try
            {
                return carritoDAO.ConfirmarCarrito(carritoId, total);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al confirmar carrito: " + ex.Message);
            }
        }

        [WebMethod(Description = "Cancela un carrito activo")]
        public bool CancelarCarrito(int carritoId)
        {
            try
            {
                return carritoDAO.CancelarCarrito(carritoId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar carrito: " + ex.Message);
            }
        }

        // ============================================================
        // SECCIÓN 4: AUTENTICACIÓN Y ROLES (ADMIN / CLIENTE)
        // ============================================================
        [WebMethod(Description = "Registra un nuevo cliente en el sistema")]
        public bool RegistrarCliente(string nombre, string apellido, string email, string password)
        {
            // Crear nuevo contexto limpio en cada invocación
            try
            {
                // Forzar limpieza previa para evitar que quede cacheado un error anterior
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Declarar instancia local si no existe
                var dao = new UsuarioDAO();

                nombre = (nombre ?? "").Trim();
                apellido = (apellido ?? "").Trim();
                email = (email ?? "").Trim().ToLower();
                password = (password ?? "").Trim();

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
                // EJECUTAR REGISTRO
                // -------------------------
                bool resultado = dao.CrearUsuario(nombre, apellido, email, password);

                // ⚠️ Limpiar cualquier resto de ejecución anterior
                GC.Collect();
                GC.WaitForPendingFinalizers();

                return resultado;
            }
            catch (Exception ex)
            {
                // ⚠️ Importante: limpiar memoria y lanzar excepción actualizada
                GC.Collect();
                GC.WaitForPendingFinalizers();
                throw new Exception("Error al registrar cliente: " + ex.Message);
            }
        }



        [WebMethod(Description = "Inicia sesión y devuelve datos del usuario con su rol")]
        public Usuario Login(string email, string password)
        {
            try
            {
                return logicaUsuarios.Login(email, password);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar sesión: " + ex.Message);
            }
        }

   
    }
}
