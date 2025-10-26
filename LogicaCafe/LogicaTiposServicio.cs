using System;
using System.Collections.Generic;
using AccesoDatosCafe;

namespace LogicaCafe
{
    public class LogicaTiposServicio
    {
        private readonly TiposServicioDAO dao = new TiposServicioDAO();

        // ✅ Obtener todos los tipos de servicio
        public List<TipoServicio> ObtenerTipos()
        {
            try
            {
                return dao.ObtenerTiposServicio();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en lógica al obtener tipos de servicio: " + ex.Message);
            }
        }

        public TipoServicio ObtenerTipoPorId(int id)
        {
            try
            {
                return dao.ObtenerTipoServicioPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en lógica al obtener tipo por ID: " + ex.Message);
            }
        }
    }
}
