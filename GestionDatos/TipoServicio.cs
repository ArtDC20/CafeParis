using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDatos
{
    public class TipoServicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string SubTipo { get; set; }
        public bool Activo { get; set; }
        public string CreadoEn { get; set; }
    }
}

