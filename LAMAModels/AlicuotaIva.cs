using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class AlicuotaIva:ICloneable
    {
        public int AlicuotaIvaId { get; set; }
        public string Descripcion { get; set; }
        public float Porcentaje { get; set; }
        public byte[] RowVersion { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
