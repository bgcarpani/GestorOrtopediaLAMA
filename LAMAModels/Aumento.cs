using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Aumento
    {
        public Tipo tipo { get; set; }

        public decimal porcentaje { get; set; }

        public bool precioVenta { get; set; }

        public bool precioAlquiler { get; set; }
    }
}
