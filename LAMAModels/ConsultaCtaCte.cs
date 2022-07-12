using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class ConsultaCtaCte
    {
        public int CtaCteId { get; set; }
        public Cliente Cliente { get; set; }

        public decimal Saldo { get; set; }

        public List<CtaCte> DetalleCtaCte { get; set; }

        public ConsultaCtaCte ()
        {
            DetalleCtaCte = new List<CtaCte>();
        }

        public override string ToString()
        {
            return $"{Cliente.Nombre} {Cliente.Apellido}";
        }
    }

    

    
}
