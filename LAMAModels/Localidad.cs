using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Localidad:ICloneable
    {
        public int LocalidadId { get; set; }
        public string Descripcion { get; set; }
        public Provincia Provincia { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
