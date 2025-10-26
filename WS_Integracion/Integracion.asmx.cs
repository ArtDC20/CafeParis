using System;
using System.Collections.Generic;
using System.Web.Services;
using LogicaCafe;
using AccesoDatosCafe;

namespace WS_Integracion
{
    [WebService(Namespace = "http://cafeteria.com/integracion")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Integracion : WebService
    {
        private readonly LogicaTiposServicio logicaTipos = new LogicaTiposServicio();

        
        [WebMethod(Description = "Búsqueda unificada de servicios (cafés, postres, desayunos, etc.)")]
        public List<TipoServicio> BuscarServicios()
        {
            try
            {
                return logicaTipos.ObtenerTipos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar servicios: " + ex.Message);
            }
        }

       
        [WebMethod(Description = "Obtiene el detalle de un tipo de servicio por su ID")]
        public TipoServicio ObtenerDetalleServicio(int id)
        {
            try
            {
                return logicaTipos.ObtenerTipoPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener detalle del servicio: " + ex.Message);
            }
        }

        // ========== 3. VERIFICAR DISPONIBILIDAD ==========
        [WebMethod(Description = "Verifica la disponibilidad de un producto o servicio")]
        public bool VerificarDisponibilidad(int idServicio, int unidades)
        {
            try
            {
                // Aquí podrías usar un método que verifique el stock real
                return unidades > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar disponibilidad: " + ex.Message);
            }
        }

        // ========== 4. COTIZAR RESERVA ==========
        [WebMethod(Description = "Calcula el total aproximado de una reserva o compra")]
        public string CotizarReserva(decimal precioUnitario, int cantidad)
        {
            try
            {
                decimal total = precioUnitario * cantidad;
                return $"Total estimado: ${total:F2}";
            }
            catch (Exception ex)
            {
                return "Error al cotizar reserva: " + ex.Message;
            }
        }

        // ========== 5. CREAR PRERESERVA ==========
        [WebMethod(Description = "Crea una pre-reserva temporal (sin confirmar)")]
        public string CrearPreReserva(string cliente, string producto, int minutos)
        {
            try
            {
                string preBookingId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                DateTime expiracion = DateTime.Now.AddMinutes(minutos);

                return $"PreReserva creada para {cliente}: ID={preBookingId}, expira en {expiracion}";
            }
            catch (Exception ex)
            {
                return "Error al crear pre-reserva: " + ex.Message;
            }
        }

        // ========== 6. CONFIRMAR RESERVA ==========
        [WebMethod(Description = "Confirma una reserva y genera un comprobante")]
        public string ConfirmarReserva(string preBookingId, string metodoPago, decimal monto)
        {
            try
            {
                return $"Reserva {preBookingId} confirmada con método {metodoPago} por ${monto:F2}";
            }
            catch (Exception ex)
            {
                return "Error al confirmar reserva: " + ex.Message;
            }
        }

        // ========== 7. CANCELAR RESERVA ==========
        [WebMethod(Description = "Cancela una reserva existente")]
        public bool CancelarReserva(string bookingId, string motivo)
        {
            try
            {
                // Aquí podrías hacer la lógica real de cancelación
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar reserva: " + ex.Message);
            }
        }

    }
}
