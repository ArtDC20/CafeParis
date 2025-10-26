using System;
using MySql.Data.MySqlClient;

namespace AccesoDatosCafe
{
    public class CarritoDAO
    {
        public int CrearCarrito(int usuarioId)
        {
            var conexion = new Conexion();
            conexion.Abrir();

            using (var con = conexion.ObtenerConexion())
            {
                var cmd = new MySqlCommand("INSERT INTO carritos (usuario_id, estado, total) VALUES (@u, 'ABIERTO', 0)", con);
                cmd.Parameters.AddWithValue("@u", usuarioId);
                cmd.ExecuteNonQuery();
                conexion.Cerrar();
                return (int)cmd.LastInsertedId;
            }
        }

        public bool AgregarItem(int carritoId, int productoId, int cantidad, decimal precio)
        {
            var conexion = new Conexion();
            conexion.Abrir();

            using (var con = conexion.ObtenerConexion())
            {
                var cmd = new MySqlCommand(@"INSERT INTO carrito_items (carrito_id, producto_id, cantidad, precio_unit)
                                             VALUES (@c, @p, @cant, @prec)", con);
                cmd.Parameters.AddWithValue("@c", carritoId);
                cmd.Parameters.AddWithValue("@p", productoId);
                cmd.Parameters.AddWithValue("@cant", cantidad);
                cmd.Parameters.AddWithValue("@prec", precio);
                var filas = cmd.ExecuteNonQuery();
                conexion.Cerrar();
                return filas > 0;
            }
        }

        public bool ConfirmarCarrito(int carritoId, decimal total)
        {
            var conexion = new Conexion();
            conexion.Abrir();

            using (var con = conexion.ObtenerConexion())
            {
                var cmd = new MySqlCommand("UPDATE carritos SET estado='CONFIRMADO', total=@t WHERE id=@id", con);
                cmd.Parameters.AddWithValue("@t", total);
                cmd.Parameters.AddWithValue("@id", carritoId);
                var filas = cmd.ExecuteNonQuery();
                conexion.Cerrar();
                return filas > 0;
            }
        }

        public bool CancelarCarrito(int carritoId)
        {
            var conexion = new Conexion();
            conexion.Abrir();

            using (var con = conexion.ObtenerConexion())
            {
                var cmd = new MySqlCommand("UPDATE carritos SET estado='CANCELADO' WHERE id=@id", con);
                cmd.Parameters.AddWithValue("@id", carritoId);
                var filas = cmd.ExecuteNonQuery();
                conexion.Cerrar();
                return filas > 0;
            }
        }
    }
}
