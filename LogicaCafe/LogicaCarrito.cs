using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatosCafe;

namespace LogicaCafe
{
    public class LogicaCarrito
    {
        private readonly CarritoDAO datos = new CarritoDAO();

        public int CrearCarrito(int usuarioId)
        {
            return datos.CrearCarrito(usuarioId);
        }

        public bool AgregarItem(int carritoId, int productoId, int cantidad, decimal precioUnit)
        {
            if (cantidad <= 0) throw new Exception("La cantidad debe ser mayor que cero.");
            return datos.AgregarItem(carritoId, productoId, cantidad, precioUnit);
        }

        public bool ConfirmarCarrito(int carritoId, decimal total)
        {
            return datos.ConfirmarCarrito(carritoId, total);
        }

        public bool CancelarCarrito(int carritoId)
        {
            return datos.CancelarCarrito(carritoId);
        }
    }
}
