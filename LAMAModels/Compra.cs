using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Compra:ICloneable
    {
        public int CompraId { get; set; }
        public DateTime FechaCompra { get; set; }
        public Proveedor Proveedor { get; set; }
        public byte[] RowVersion { get; set; }
        public decimal ValorIva { get; set; }
        public decimal ValorNeto { get; set; }

        public decimal Total { get; set; }
        public List<DetalleCompra> DetallesCompras { get; set; }

        public Compra()
        {
            DetallesCompras=new List<DetalleCompra>();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

  
}
