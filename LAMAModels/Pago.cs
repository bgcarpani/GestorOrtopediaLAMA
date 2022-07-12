using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Pago:ICloneable
    {
        public int PagoId { get; set; }
        public Cliente Cliente { get; set; }
        public CtaCte CtaCte { get; set; }
        public string Descripcion { get; set; }
        public FormaDePago FormaDePago { get; set; }
        public decimal Importe { get; set; }

        public DateTime FechaPago { get; set; }

        public decimal ImporteOS { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
