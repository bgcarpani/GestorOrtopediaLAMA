using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Marca:ICloneable
    {
        public int MarcaId { get; set; }
        public string Descripcion { get; set; }
        //Implementación de la interfase Icloneable
        //Crea una copia del objeto
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
