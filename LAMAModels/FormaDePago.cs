using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class FormaDePago:ICloneable
    {
        public int FormaDePagoId { get; set; }
        public string Descripcion { get; set; }
        public byte[] RowVersion { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
