using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Transaccion
    {
        public int TransaccionId { get; set; }

        public TipoTransaccion TipoTransaccion { get; set; }

        public Cliente Cliente { get; set; }

        public DateTime FechaTransaccion { get; set; }

        public decimal Importe { get; set; }
    }
}
