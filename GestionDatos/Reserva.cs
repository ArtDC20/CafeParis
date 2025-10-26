using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDatos
{
    public class Reserva
    {
        public long Id { get; set; }
        public int UsuarioId { get; set; }
        public string Estado { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public decimal Total { get; set; }
        public string CreadoEn { get; set; }
    }
}
