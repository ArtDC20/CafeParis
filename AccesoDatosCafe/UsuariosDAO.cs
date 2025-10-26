using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AccesoDatosCafe
{
    public class UsuarioDAO
    {
        private readonly Conexion conexion = new Conexion();

        // ============================================================
        // LISTAR USUARIOS
        // ============================================================
        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (var conn = conexion.ObtenerConexion())
                {
                    conexion.Abrir();

                    string sql = @"SELECT u.id, u.nombre, u.apellido, u.email, r.nombre AS rol 
                                   FROM usuarios u
                                   INNER JOIN roles r ON u.rol_id = r.id
                                   WHERE u.estado = 1";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Usuario
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido"),
                                Email = reader.GetString("email"),
                                Rol = reader.GetString("rol")
                            });
                        }
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar usuarios: " + ex.Message);
            }
            finally
            {
                conexion.Cerrar();
            }
        }

        // ============================================================
        // REGISTRAR NUEVO CLIENTE (con verificación de duplicado)
        // ============================================================
        public bool CrearUsuario(string nombre, string apellido, string email, string password)
        {
            try
            {
                using (var conn = conexion.ObtenerConexion())
                {
                    conexion.Abrir();

                    // ✅ Verificar si el correo ya existe antes de registrar
                    string checkSql = "SELECT COUNT(*) FROM usuarios WHERE email = @correo";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@correo", email);

                    long existe = (long)checkCmd.ExecuteScalar();
                    if (existe > 0)
                    {
                        throw new Exception("El correo electrónico ya está registrado. Use otro correo.");
                    }

                    // ✅ Insertar nuevo usuario con rol_id = 2 (CLIENTE)
                    string insertSql = @"INSERT INTO usuarios (nombre, apellido, email, password, rol_id, estado)
                                         VALUES (@nombre, @apellido, @correo, @pass, 2, 1)";

                    MySqlCommand cmd = new MySqlCommand(insertSql, conn);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                    cmd.Parameters.AddWithValue("@correo", email);
                    cmd.Parameters.AddWithValue("@pass", password);

                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                    throw new Exception("El correo ya existe en la base de datos.");
                else
                    throw new Exception("Error en MySQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar usuario: " + ex.Message);
            }
            finally
            {
                conexion.Cerrar();
            }
        }

        // ============================================================
        // LOGIN
        // ============================================================
        public Usuario Login(string email, string password)
        {
            try
            {
                using (var conn = conexion.ObtenerConexion())
                {
                    conexion.Abrir();

                    string sql = @"SELECT u.id, u.nombre, u.apellido, u.email, r.nombre AS rol 
                                   FROM usuarios u
                                   INNER JOIN roles r ON u.rol_id = r.id
                                   WHERE u.email = @correo AND u.password = @pass AND u.estado = 1";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@correo", email);
                    cmd.Parameters.AddWithValue("@pass", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido"),
                                Email = reader.GetString("email"),
                                Rol = reader.GetString("rol")
                            };
                        }
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar sesión: " + ex.Message);
            }
            finally
            {
                conexion.Cerrar();
            }
        }
    }

    // ============================================================
    // MODELO USUARIO
    // ============================================================
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }
}
