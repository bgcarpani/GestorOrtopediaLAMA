using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Venta:ICloneable
    {
        public int VentaId { get; set; }
        public DateTime FechaVenta { get; set; }
        public Cliente Cliente { get; set; }
        public decimal Total { get; set; }
        public List<DetalleVenta> Detalle { get; set; }

        public bool EsConsumidorFinal { get; set; }

        public bool Devuelto { get; set; }

        public decimal ImporteOS { get; set; }

        public Venta()
        {
            Detalle = new List<DetalleVenta>();
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
