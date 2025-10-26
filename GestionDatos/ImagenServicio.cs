using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDatos
{
    public class ImagenServicio
    {
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public string Url { get; set; }
        public string CreadoEn { get; set; }
    }
}

