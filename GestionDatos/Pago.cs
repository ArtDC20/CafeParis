using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDatos
{
    public class Pago
    {
        public long Id { get; set; }
        public long ReservaId { get; set; }
        public int MetodoId { get; set; }
        public string RefExterna { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public string CreadoEn { get; set; }
    }
}

