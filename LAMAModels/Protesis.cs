using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Protesis:ICloneable
    {
        public int ProtesisId { get; set; }

        public string NombreProtesis
        {
            get { return $"{Tipo} {Descripcion}"; }
        }

        public string Tipo { get; set; }

        public string Descripcion { get; set; }

        public decimal Importe { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
